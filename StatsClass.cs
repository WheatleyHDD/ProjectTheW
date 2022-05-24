namespace ProjectTheW
{
    internal class StatsClass
    {
        public static int PlayerHealth { get; set; } = 5;
        public static int BulletDamage { get; set; } = 1;
        public static int BulletPush { get; set; } = 0;
        public static float BulletSpeed { get; set; } = 300f;
        public static float PlayerSpeed { get; set; } = 190f;

        public static int WeaponType { get; set; } = 0;
        public static int Level { get; set; } = 1;

        public static void ResetAll()
        {
            PlayerHealth = 5;
            BulletDamage = 1;
            BulletPush = 0;
            BulletSpeed = 300f;
            PlayerSpeed = 190f;
            Level = 1;
        }

        public static bool AddToStat(int num)
        {
            if (PlayerHealth < 3) return false;
            switch (num)
            {
                case 0:
                    BulletDamage += 1;
                    goto default;
                case 1:
                    BulletSpeed += 50f;
                    goto default;
                case 2:
                    BulletPush += 1;
                    goto default;
                case 3:
                    PlayerSpeed += 10f;
                    goto default;
                default:
                    PlayerHealth -= 1;
                    break;
            }
            return true;
        }

        public static void AddToStatInGame(int num)
        {
            switch (num)
            {
                case 0:
                    BulletDamage += 1;
                    break;
                case 1:
                    BulletSpeed += 50f;
                    break;
                case 2:
                    BulletPush += 1;
                    break;
                case 3:
                    PlayerSpeed += 10f;
                    break;
                case 4:
                    PlayerHealth++;
                    break;
            }
        }
    }
}
