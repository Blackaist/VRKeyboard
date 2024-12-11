using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

namespace Project
{
	public class KeyboardInputController : MonoBehaviour
	{
		private const float _triggerThreshold = 0.2f;

		[SerializeField] private XRInputValueReader<float> _confirmButton = new XRInputValueReader<float>("Confirm button");

		private Selectable _firstSelectable = null;

		private bool _isPressed = false;

		private void Awake()
		{
			_firstSelectable = GetComponentInChildren<Button>(true);
		}

		private void OnEnable()
		{
			_firstSelectable.Select();
		}

		private void Update()
		{
			if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null ||
				(!EventSystem.current.alreadySelecting && EventSystem.current.currentSelectedGameObject.transform.parent.GetHashCode() != transform.GetHashCode()))
			{
				_firstSelectable.Select();
			}

			if (!_isPressed && IsPressed())
			{
				_isPressed = true;
				EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
			}
			else if (IsReleased())
			{
				_isPressed = false;
			}
		}

		/*
		* ������ ���� ����� ������� XRInptValueReader ����� ������������� �� �����-�� Input Controller � ����������� �������. � ������ ��������� ������� ���� ��������.
		* ������������ Update �� ����� ������ �������. ������� �������� �� event ���� �� ����� ������.
		* _triggerThreshold �����-�� ���������� ��������� � �� ������ ��������� �����. ���� ������� �������� ������ GameConsts ����� ��� �������� const ��������.
		* ���� ������ ��������, �� ������ ������������� � �������� ������� �� ���.
		* 
		* ������ �������� �� ����� ����. ����� ������������ ����������� ���������� ��� ���� ����� ������ �� Grab. � ������� ��� G. ���� ��� ���������������� Enter (XRI UI) � Event System
		* ����� � � ������� ������� � InputField, ������� ���� ��� Tab (����� ����� ���� ���� �������) � ����� �� ����������.
		*/
		private bool IsPressed() => _confirmButton.ReadValue() > _triggerThreshold ? true : false;
		private bool IsReleased() => _confirmButton.ReadValue() <= _triggerThreshold ? true : false;
	}

}
