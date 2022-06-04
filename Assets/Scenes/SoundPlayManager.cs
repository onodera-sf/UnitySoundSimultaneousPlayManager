using System.Collections.Generic;
using UnityEngine;

/// <summary>音声再生管理クラスです。</summary>
public class SoundPlayManager : MonoBehaviour
{
  /// <summary>１つの種類の音声の再生情報を保持するクラスです。</summary>
  private class PlayInfo
  {
    /// <summary>再生している AudioSource の一覧です。</summary>
    public AudioSource[] AudioSource { get; set; }

    /// <summary>現在の再生インデックスです。</summary>
    public int NowIndex { get; set; }
  }

  /// <summary>再生しようとしている音声データのキューです。</summary>
  private HashSet<AudioClip> Queue = new HashSet<AudioClip>();

  /// <summary>再生している音声を管理している一覧です。</summary>
  private Dictionary<AudioClip, PlayInfo> Sources = new Dictionary<AudioClip, PlayInfo>();

  /// <summary>
  /// 同一音声同時再生最大数。
  /// </summary>
  [SerializeField, Range(1, 32)] private int MaxSimultaneousPlayCount = 2;

  protected void Update()
  {
    foreach (var item in Queue)
    {
      AudioSource source;
      if (Sources.ContainsKey(item) == false)
      {
        // 一度も再生されていない Clip がある場合は PlayInfo を生成します
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
        // 再生に使用する AudioSource を順番に取得します
        var info = Sources[item];
        info.NowIndex = (info.NowIndex + 1) % MaxSimultaneousPlayCount;
        source = info.AudioSource[info.NowIndex];
      }

      source.Play();
    }
    Queue.Clear();
  }

  /// <summary>
  /// 効果音を再生します。
  /// </summary>
  public void Play(AudioClip clip)
  {
    // 同一フレームで複数再生しないようにすでにキューに入っているか確認します
    if (Queue.Contains(clip) == false)
    {
      Queue.Add(clip);
    }
  }

  public void OnDestroy()
  {
    // 不要になった参照をすべて外します
    foreach (var source in Sources)
    {
      source.Value.AudioSource = null;
    }
    Sources.Clear();
    Queue.Clear();
  }
}
