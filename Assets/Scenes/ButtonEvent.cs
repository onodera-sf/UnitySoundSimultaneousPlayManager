using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
	/// <summary>�Đ����鉹���f�[�^�B</summary>
	[SerializeField] private AudioClip AudioClip;

	/// <summary>���삵�������Đ��Ǘ��N���X�B</summary>
	[SerializeField] private SoundPlayManager SoundPlayManager;

	/// <summary>�{�^�����N���b�N�����Ƃ��B</summary>
	public void OnClick()
	{
		SoundPlayManager.Play(AudioClip);
	}
}
