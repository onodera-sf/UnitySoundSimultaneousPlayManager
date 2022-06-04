using System.Collections.Generic;
using UnityEngine;

/// <summary>�����Đ��Ǘ��N���X�ł��B</summary>
public class SoundPlayManager : MonoBehaviour
{
  /// <summary>�P�̎�ނ̉����̍Đ�����ێ�����N���X�ł��B</summary>
  private class PlayInfo
  {
    /// <summary>�Đ����Ă��� AudioSource �̈ꗗ�ł��B</summary>
    public AudioSource[] AudioSource { get; set; }

    /// <summary>���݂̍Đ��C���f�b�N�X�ł��B</summary>
    public int NowIndex { get; set; }
  }

  /// <summary>�Đ����悤�Ƃ��Ă��鉹���f�[�^�̃L���[�ł��B</summary>
  private HashSet<AudioClip> Queue = new HashSet<AudioClip>();

  /// <summary>�Đ����Ă��鉹�����Ǘ����Ă���ꗗ�ł��B</summary>
  private Dictionary<AudioClip, PlayInfo> Sources = new Dictionary<AudioClip, PlayInfo>();

  /// <summary>
  /// ���ꉹ�������Đ��ő吔�B
  /// </summary>
  [SerializeField, Range(1, 32)] private int MaxSimultaneousPlayCount = 2;

  protected void Update()
  {
    foreach (var item in Queue)
    {
      AudioSource source;
      if (Sources.ContainsKey(item) == false)
      {
        // ��x���Đ�����Ă��Ȃ� Clip ������ꍇ�� PlayInfo �𐶐����܂�
        var info = new PlayInfo()
        {
          AudioSource = new AudioSource[MaxSimultaneousPlayCount],
        };
        for (int i = 0; i < MaxSimultaneousPlayCount; i++)
        {
          var s = gameObject.AddComponent<AudioSource>();
          s.clip = item;
          info.AudioSource[i] = s;
        }
        Sources.Add(item, info);
        source = info.AudioSource[0];
      }
      else
      {
        // �Đ��Ɏg�p���� AudioSource �����ԂɎ擾���܂�
        var info = Sources[item];
        info.NowIndex = (info.NowIndex + 1) % MaxSimultaneousPlayCount;
        source = info.AudioSource[info.NowIndex];
      }

      source.Play();
    }
    Queue.Clear();
  }

  /// <summary>
  /// ���ʉ����Đ����܂��B
  /// </summary>
  public void Play(AudioClip clip)
  {
    // ����t���[���ŕ����Đ����Ȃ��悤�ɂ��łɃL���[�ɓ����Ă��邩�m�F���܂�
    if (Queue.Contains(clip) == false)
    {
      Queue.Add(clip);
    }
  }

  public void OnDestroy()
  {
    // �s�v�ɂȂ����Q�Ƃ����ׂĊO���܂�
    foreach (var source in Sources)
    {
      source.Value.AudioSource = null;
    }
    Sources.Clear();
    Queue.Clear();
  }
}
