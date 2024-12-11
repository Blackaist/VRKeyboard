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
		* Вместо того чтобы хранить XRInptValueReader лучше подписываться на какой-то Input Controller к конкретному событию. В рамках Тестового задания было упрощено.
		* Использовать Update не самый лучший вариант. Поэтому подписка на event была бы здесь кстати.
		* _triggerThreshold какая-то глобальная константа и не должна храниться здесь. Есть вариант создания класса GameConsts чисто для хранения const значений.
		* Есть другие варианты, но смысла рассматривать в тестовом задании их нет.
		* 
		* Проект настроен на левую руку. Чтобы использовать виртуальную клавиатуру без луча нужно нажать на Grab. В эдиторе это G. Есть еще переопределенный Enter (XRI UI) в Event System
		* Лично я в эдиторе подхожу к InputField, нажимаю один раз Tab (чтобы левая рука была активна) и тыкаю на клавиатуру.
		*/
		private bool IsPressed() => _confirmButton.ReadValue() > _triggerThreshold ? true : false;
		private bool IsReleased() => _confirmButton.ReadValue() <= _triggerThreshold ? true : false;
	}

}
