using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
	/// <summary>再生する音声データ。</summary>
	[SerializeField] private AudioClip AudioClip;

	/// <summary>自作した音声再生管理クラス。</summary>
	[SerializeField] private SoundPlayManager SoundPlayManager;

	/// <summary>ボタンをクリックしたとき。</summary>
	public void OnClick()
	{
		SoundPlayManager.Play(AudioClip);
	}
}
