using System.Collections;
using MoreMountains.Feedbacks;
using Shapes;
using UnityEngine;

namespace _Game.Scripts.Feedbacks
{    
	[AddComponentMenu("")]
	[FeedbackHelp("Animates a Shapes Color over time")]
	[FeedbackPath("Shapes/Color")]
	public class ShapesColorFeedback : MMFeedback
	{
		public enum Mode { Direct, PingPong }
		public enum TimeScales { Scaled, Unscaled }
		
		
		[Header("Target")]
		[SerializeField] private ShapeRenderer _renderer;
		[SerializeField] public TimeScales TimeScale = TimeScales.Scaled;
		[SerializeField] private float _duration;

		
		[Header("Color")]
		[SerializeField] private Mode _TravelMode;
		[SerializeField] private bool _resetToOriginalColor = true;
		[SerializeField] private Gradient _ramp;

		
		private Color _initialColor;
		
		
		public override float FeedbackDuration { get { return _duration; } }
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
		{
			if (!Active) return;

			_initialColor = _renderer.Color;
			
			switch (_TravelMode)
			{
				case Mode.Direct:
					StartCoroutine(DirectGradient());
					break;
				case Mode.PingPong:
					StartCoroutine(PingPongGradient());
					break;
			}
		}


		private IEnumerator DirectGradient()
		{
			float currentValue = 0f;

			while (currentValue < _duration)
			{
				float percent = Mathf.Clamp01(currentValue / _duration);
				
				_renderer.Color = _ramp.Evaluate(percent);
				
				currentValue += (TimeScale == TimeScales.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime;
				yield return null;
			}
			
			if (_resetToOriginalColor)
				_renderer.Color = _initialColor;
			else
				_renderer.Color = _ramp.Evaluate(1);
		}
		
		private IEnumerator PingPongGradient()
		{
			float stepTime = 0f;

			float pingTime = _duration / 2;
			

			while (stepTime < pingTime)
			{
				float percent = Mathf.Clamp01(stepTime / pingTime);
				
				_renderer.Color = _ramp.Evaluate(percent);
				
				stepTime += (TimeScale == TimeScales.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime;
				yield return null;
			}

			stepTime = 0;
			
			while (stepTime < pingTime)
			{
				float percent = Mathf.Clamp01(1 - (stepTime / pingTime));
				
				_renderer.Color = _ramp.Evaluate(percent);
				
				stepTime += (TimeScale == TimeScales.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime;
				yield return null;
			}


			if (_resetToOriginalColor)
				_renderer.Color = _initialColor;
			else
				_renderer.Color = _ramp.Evaluate(0);
		}
		

		protected override void CustomStopFeedback(Vector3 position, float attenuation = 1)
		{
			if (_resetToOriginalColor)
				_renderer.Color = _initialColor;
			base.CustomStopFeedback(position, attenuation);
		}
	}
}