namespace MainGame
{
    public class FistBullet : Bullet
    {
        public override void StartMoving(float speed, float secondsToDisappear)
        {
            Destroy(gameObject, secondsToDisappear);
        }
    }
}
