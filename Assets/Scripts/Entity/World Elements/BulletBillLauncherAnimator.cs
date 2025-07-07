using Quantum;
using UnityEngine;
using static NSMB.Utilities.QuantumViewUtils;

namespace NSMB.Entities.World {
    public class BulletBillLauncherAnimator : BreakableObjectAnimator {

        //---Serialized Variables
        [SerializeField] private Animation headAnimation;
        [SerializeField] private SpriteRenderer headRenderer;
        [SerializeField] private Transform headOrigin;
        [SerializeField] private ParticleSystem bulletBillShoot;

        public override void Start() {
            base.Start();
            QuantumEvent.Subscribe<EventBulletBillLauncherShoot>(this, OnBulletBillLauncherShoot, FilterOutReplayFastForward);
        }

        public override unsafe void OnUpdateView() {
            base.OnUpdateView();

            Frame f = Game.Frames.Verified;
            if (!f.Exists(EntityRef)
                || f.Global->GameState < GameState.Playing) {
                return;
            }

            var breakable = f.Unsafe.GetPointer<BreakableObject>(EntityRef);
            headRenderer.enabled = breakable->CurrentHeight > 0;
            headOrigin.transform.localPosition = Vector3.up * (breakable->CurrentHeight.AsFloat * 0.5f);
        }

        protected override void OnBreakableObjectBroken(EventBreakableObjectBroken e) {
            if (e.Entity != EntityRef) {
                return;
            }

            base.OnBreakableObjectBroken(e);
            headRenderer.enabled = false;
        }

        private unsafe void OnBulletBillLauncherShoot(EventBulletBillLauncherShoot e) {
            if (e.Entity != EntityRef) {
                return;
            }

            headAnimation.Play();
            bulletBillShoot.transform.position = headOrigin.position + (e.Right ? new Vector3(0.25f, 0.25f, 0) : new Vector3(-0.25f, 0.25f, 0));
            bulletBillShoot.Play();
        }
    }
}
