using DG.Tweening;

public static class Delay
{
    public static Tweener For(float duration)
    {
        var temp = 0;
        return DOTween.To(() => temp, x => temp = x, 1, duration);
    }
}
