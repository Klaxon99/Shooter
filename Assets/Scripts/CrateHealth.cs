public class CrateHealth : AbstractHealth
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}