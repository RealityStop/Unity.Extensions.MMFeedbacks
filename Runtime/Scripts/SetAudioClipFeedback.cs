using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Game.Scripts.Feedbacks
{    
	[AddComponentMenu("")]
	[FeedbackPath("Audio/AudioSource Clip")]
	[FeedbackHelp("This feedback lets you set the clip of a target AudioSource.")]
	public class SetAudioClipFeedback : MMFeedback
	{      
		/// sets the inspector color for this feedback
#if UNITY_EDITOR
		public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.SoundsColor; } }
#endif

		[Header("Audio")]
		public AudioSource TargetSource;
		public AudioClip TargetClip;
		
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
		{
			if (Active)
			{
				if (TargetSource == null)
					TargetSource = GetComponent<AudioSource>();

				if (TargetSource != null)
					TargetSource.clip = TargetClip;
			}
		}
	}
}