using UnityEngine;

namespace CCUtil.Projectiles{
  public interface IProjectileReceiver
  {
    public void ReceiveProjectile(ProjectileData data, ProjectileCollisionData collision);
  }
}
