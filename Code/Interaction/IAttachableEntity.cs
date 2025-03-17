public interface IAttachableEntity
{
	public enum AttachableEntityType {
		Weapon
	}

	void OnAttach( Transform eyeTransform );
	void OnDrop( Transform eyeTransform );

	Vector3 GetAttachOffset();

	AttachableEntityType OfAttachableType();
	GameObject GetGameObject();

}
