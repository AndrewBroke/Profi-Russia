using UnityEngine;

namespace Watermelon
{
    public class WheelPartBehavior : RoverPartBehavior
    {
        [SerializeField] protected WheelJoint2D wheelJoint;
        [SerializeField]  MeshRenderer rendererRef;

        [SerializeField] float force;
        [SerializeField] protected  Collider2D colliderRef;


        public override void Connect(RoverPart data, RoverBehavior master, SnapPoint point)
        {
            base.Connect(data, master, point);

            wheelJoint.anchor = Vector2.zero;
            wheelJoint.connectedBody = master.Rigidbody;
            wheelJoint.connectedAnchor = point.transform.localPosition;

            startPos = wheelJoint.transform.localPosition;

            rendererRef.enabled = true;
        }


        public override void Disconect(bool immediately)
        {
            base.Disconect(immediately);

            wheelJoint.connectedBody = null;
        }


        public virtual void DriveUpdate()
        {
            if (Physics2D.IsTouchingLayers(colliderRef, PhysicsHelper.GetLayerMask(PhysicsLayer.Ground)))
            {
                Master.AddForce(Master.transform.right * force);
            }

            var suspension = wheelJoint.suspension;

            suspension.dampingRatio = Mathf.Clamp01(Mathf.InverseLerp(0, 200, Master.Rigidbody.velocity.sqrMagnitude)) * 0.3f + 0.7f;
            suspension.frequency = Mathf.Clamp01(Mathf.InverseLerp(0, 200, Master.Rigidbody.velocity.sqrMagnitude)) * 100 + 20;

            wheelJoint.suspension = suspension;

        }


        public override void Explode()
        {
            base.Explode();
            rendererRef.enabled = false;

            wheelJoint.connectedBody = null;
        }


    }
}