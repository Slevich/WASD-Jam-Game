using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(255f / 255f, 249f / 255f, 203 / 255f)]
[TrackBindingType(typeof(TrackContainer))]
[TrackClipType(typeof(NoteClip))]
public class NoteTrack : TrackAsset
{
}
