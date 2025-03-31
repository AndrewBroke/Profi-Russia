using UnityEngine;

namespace Watermelon
{
    public class SnapPoint : MonoBehaviour
    {
        [SerializeField] Material redMaterial;
        [SerializeField] Material greenMaterial;
        [SerializeField] Material grayMaterial;

        [Space]
        [SerializeField] new MeshRenderer renderer;

        [Space]
        [SerializeField] Collider snapCollider;

        [Space]
        [SerializeField] Transform forcePoint;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] SpriteRenderer shadowSprite;

        public Transform ForcePoint => forcePoint;

        public int Id { get; set; }

        public bool IsActive => renderer.enabled;

        TweenCase spriteSizeCase;

        public bool isRed = true;


        private void Awake()
        {
            MakeRed();
        }


        public void Init()
        {
            var viewportPos = CameraController.MainCamera.WorldToViewportPoint(transform.position);
            var ray = CameraController.MainCamera.ViewportPointToRay(viewportPos);

            var k = (ray.origin.z - (CameraController.MainCamera.transform.position.z - 3)) / ray.direction.z;

            var position = ray.origin + ray.direction * k;

            sprite.transform.position = position;
            sprite.transform.rotation = Quaternion.LookRotation(-ray.direction);
        }


        public void MakeRed()
        {
            renderer.material = redMaterial;

            if (!isRed)
            {
                if (spriteSizeCase != null && spriteSizeCase.isActive)
                {
                    spriteSizeCase.Kill();
                }

                spriteSizeCase = sprite.DOAction((start, end, time) =>
                {
                    sprite.size = start + (end - start) * time;
                    shadowSprite.size = sprite.size;
                }, sprite.size, Vector2.one * 0.3f, 0.2f).SetEasing(Ease.Type.SineInOut);
            }

            isRed = true;
        }


        public void MakeGreen()
        {
            renderer.material = greenMaterial;

            if (isRed)
            {
                if (spriteSizeCase != null && spriteSizeCase.isActive)
                {
                    spriteSizeCase.Kill();
                }

                spriteSizeCase = sprite.DOAction((start, end, time) =>
                {
                    sprite.size = start + (end - start) * time;
                    shadowSprite.size = sprite.size;
                }, sprite.size, Vector2.one * 0.4f, 0.2f).SetEasing(Ease.Type.SineInOut);
            }

            isRed = false;


        }


        public void SetActive(bool isActive)
        {
            sprite.enabled = isActive;

            if (isActive || snapCollider.enabled)
            {
                shadowSprite.enabled = isActive;
                renderer.enabled = isActive;

                if (!isActive)
                {
                    sprite.size = Vector2.zero;
                    shadowSprite.size = Vector2.zero;
                }
            }

        }


        public void DisableCollider()
        {
            snapCollider.enabled = false;

            renderer.material = grayMaterial;

            if (spriteSizeCase != null && spriteSizeCase.isActive)
            {
                spriteSizeCase.Kill();
            }

            spriteSizeCase = sprite.DOAction((start, end, time) =>
            {
                sprite.size = start + (end - start) * time;
                shadowSprite.size = sprite.size;
            }, sprite.size, Vector2.one * 0, 0.2f).SetEasing(Ease.Type.SineInOut).OnComplete(() =>
            {
                shadowSprite.enabled = false;
                renderer.enabled = false;
            });
        }

        public void EnableCollider()
        {
            snapCollider.enabled = true;
            isRed = false;
            MakeRed();
        }
    }
}