namespace ProjectTheW
{
	[Flags]
	public enum Tags
	{
		Solid = 1 << 0,
		Player = 1 << 1,
		Enemy = 1 << 2,
		Bullet = 1 << 3,
		Other = 1 << 4,
		Loot = 1 << 5,
		EBullet = 1 << 5,
	}
}
