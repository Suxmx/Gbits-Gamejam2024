using DG.Tweening;

namespace GameMain
{
    public static class TweenExtension
    {
        public static bool IsActivePlaying(this Tween tween)
        {
            return (tween is not null && tween.IsActive() && tween.IsPlaying());
        }
    }
}