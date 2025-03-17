using System;
using System.Threading;

public sealed class Pistol : Component, Component.IPressable, IAttachableEntity, IWeapon, IAnimatable
{
    [RequireComponent] public Rigidbody Rigidbody { get;set; }
	[RequireComponent] public Collider Collider { get;set; }
	[Property] public Vector3 AttachOffset; 
    [Property] public SoundEvent FXGunshot;
	[Property] public ParticleEffect FireEffect;
	[Property] public ParticleEmitter FireEmitter;
	[RequireComponent] public RecoilHandler RecoilHandler { get; set; }

	CancellationTokenSource _cancelRecoilAnimToken;

    bool _attachedToInventory = false;



	public GameObject GetGameObject()
	{
		return GameObject;
	}

	public Vector3 GetAttachOffset()
	{
		return AttachOffset;
	}

	public IAttachableEntity.AttachableEntityType OfAttachableType()
	{
		return IAttachableEntity.AttachableEntityType.Weapon;
	}

	public void OnAttach(Transform eyeTransform)
	{
        _attachedToInventory = true;
		Rigidbody.Gravity = false;
		Collider.Enabled = false;
		Rigidbody.ClearForces();
	}

	public void OnDrop(Transform eyeTransform)
	{
        _attachedToInventory = false;
		Rigidbody.Gravity = true;
		Collider.Enabled = true;
		Rigidbody.ClearForces();
		Rigidbody.ApplyForceAt(eyeTransform.Position, eyeTransform.Forward * 10f);
	}

	public bool Press( IPressable.Event e )
	{
		IInventory inventory = e.Source.GetComponent<IInventory>();
		if (inventory != null) {
			inventory.Attach(this);
			return true;
		} else {
			return false;
		}
	}

	public void Shoot()
	{
		if (!_attachedToInventory) {
			return;
        }
        
		Sound.Play(FXGunshot);
		FireEmitter.Emit(FireEffect);
		_ = RecoilHandler.Run();
	}

	public Rotation GetRotationAnim()
	{
		return RecoilHandler.GetRotationAnim();
	}

	public Vector3 GetPositionAnim()
	{
		throw new NotImplementedException();
	}
}