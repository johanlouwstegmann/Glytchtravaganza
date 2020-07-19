﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkManager : MonoBehaviour
{
	[SerializeField]
	private RectTransform _galleryContainer;
	[SerializeField]
	private GalleryView _galleryContentPrefab;
	[SerializeField]
	private int _galleryIndex = 0;
	[SerializeField]
	private List<GalleryView> _galleryItems = new List<GalleryView>();

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private Vector2 _offscreenRight
	{
		get
		{
			return UIHelper.FarOffScreen;//new Vector2(Screen.currentResolution.width / 2f + UIHelper.MinimumDisplayLength / 2f, 0f);
		}
	}
	private Vector2 _offscreenLeft
	{
		get
		{
			return UIHelper.FarOffScreen; //new Vector2(-_offscreenRight.x, 0f);
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		ArtworkController.Instance.RegisterManager(this);
		_canvasGroup.alpha = 0f;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}

	internal void ArtworkSelected(Artwork artwork)
	{
		Debug.Log(artwork.Key);
		BuildGallery(artwork);
		_canvasGroup.alpha = 1f;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
	}

	internal void BuildGallery(Artwork artwork)
	{
		ClearGallery();
		for (int i = 0; i < artwork.ArtContents.Count; i++)
		{
			_galleryItems.Add(CreateView(artwork.ArtContents[i]));
		}
		SetGalleryPosition(_galleryIndex);
	}

	private void SetGalleryPosition(int galleryIndex)
	{
		for (int i = 0; i < _galleryItems.Count; i++)
		{
			Vector2 pos = Vector2.zero;
			if (i < galleryIndex)
			{
				pos = _offscreenLeft;
			}
			else if (i > galleryIndex)
			{
				pos = _offscreenRight;
			}

			_galleryItems[i].RectTransform.anchoredPosition = pos;
		}
	}

	[ContextMenu("Next")]
	public void NextItem()
	{
		_galleryIndex = (_galleryIndex < (_galleryItems.Count - 1)) ? _galleryIndex + 1 : 0;
		SetGalleryPosition(_galleryIndex);
	}

	[ContextMenu("Prev")]
	public void PreviousItem()
	{
		_galleryIndex = (_galleryIndex > 0) ? _galleryIndex - 1 : (_galleryItems.Count - 1);
		SetGalleryPosition(_galleryIndex);
	}

	public void CloseGallery()
	{
		ArtworkController.Instance.ArtworkClosed();
		_canvasGroup.alpha = 0f;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}

	private GalleryView CreateView(ArtContent content)
	{
		GalleryView galleryView = Instantiate(_galleryContentPrefab, _galleryContainer.transform);
		galleryView.SetContent(content);
		return galleryView;
	}

	private void ClearGallery()
	{
		for (int i = 0; i < _galleryItems.Count; i++)
		{
			Destroy(_galleryItems[i].gameObject);
		}
		_galleryIndex = 0;
		_galleryItems.Clear();
	}
}