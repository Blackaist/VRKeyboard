using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using TMPro;
using UnityEngine;

namespace Project
{
	//по хорошему нужно реализовать интерфейс для каждого UI элементы с которым можно интерактировать.
	//Здесь я работаю только с input field так что это можно опустить.
	//Поле при закрытии не сохраняет текст по причине - NonNativeKeyboard.OnDisable() Clear()
	[RequireComponent(typeof(TMP_InputField))]
	public class InputKeyboardField : MonoBehaviour
	{
		private const float _maxDistance = 0.25f;
		private const float _verticalOffset = -0.5f;
		private TMP_InputField _inputField = null;

		private void Awake()
		{
			_inputField = GetComponent<TMP_InputField>();
			_inputField.onSelect.AddListener(OnOpenKeyboard);
		}

		private void OnDestroy()
		{
			_inputField.onSelect.RemoveListener(OnOpenKeyboard);
		}

		private void OnOpenKeyboard(string text)
		{
			NonNativeKeyboard.Instance.InputField = _inputField;
			NonNativeKeyboard.Instance.PresentKeyboard(_inputField.text);

			SetupKeyboard();

			//Или любая другая логика которая блокирует передвижение игрока. 
			//Почему так? Возможно игрок может двигаться во время открытия клавиатуры. В ТЗ не определено.
			Time.timeScale = 0f;
		}

		private void SetupKeyboard()
		{
			var direction = transform.forward;
			direction.y = 0;
			direction.Normalize();

			//Просто базовая логика для не самого лучшего (но простого) отображения элемента.
			var targetPosition = transform.position + direction * _maxDistance + Vector3.up * _verticalOffset;

			NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
			NonNativeKeyboard.Instance.OnClosed += OnCloseKeyboard;
		}

		private void OnCloseKeyboard(object sender, EventArgs args)
		{
			Time.timeScale = 1f;
			NonNativeKeyboard.Instance.OnClosed -= OnCloseKeyboard;
		}
	}

}
