using System;
using System.Numerics;
using Sandbox;

public sealed class PlayerManager : Component, IInventory
{
	[RequireComponent] public PlayerController Player { get; set; }
	[Property] public GameObject Arms {get; set;}
	[Property] public GameObject WeaponAttachPoint;

	IAttachableEntity _attachable = null;

	/*
	* Hook is called from other components
	*/
	public void Attach(IAttachableEntity entity)
	{
		Log.Info("Attaching entity"+entity.OfAttachableType().ToString());
		if (entity.OfAttachableType() == IAttachableEntity.AttachableEntityType.Weapon) {
			entity.OnAttach(Player.EyeTransform);
			_attachable = entity;
		}
	}

	public void Drop()
	{
		if (_attachable != null) {
			_attachable.OnDrop(Player.EyeTransform);
		}

		_attachable = null;
	}

	private Rotation GetEyeLevelRotation() {
		return Player.EyeAngles;
	}

	protected override void OnUpdate()
	{
		RenderArmsOnEyeLevel();
		UpdateAttachablePosition();
		UpdateAttachableInteraction();
		UpdateDrop();
	}

	private void UpdateDrop()
	{
		if (Input.Pressed("drop") && _attachable != null) {
			Drop();
		}
	}

	private void UpdateAttachablePosition()
	{
		if (_attachable != null && _attachable.OfAttachableType() == IAttachableEntity.AttachableEntityType.Weapon) {
			_attachable.GetGameObject().LocalPosition = WeaponAttachPoint.Transform.World.PointToWorld(WeaponAttachPoint.Transform.World.PointToLocal(WeaponAttachPoint.WorldPosition) + _attachable.GetAttachOffset());
			_attachable.GetGameObject().WorldRotation = WeaponAttachPoint.WorldRotation * Rotation.FromAxis(Vector3.Up, 90f);

			var animatable = _attachable.GetGameObject().GetComponent<IAnimatable>();
			if (animatable != null) {
				var animRotation = animatable.GetRotationAnim();
				Log.Info(animRotation);
				_attachable.GetGameObject().WorldRotation *= animRotation;
			}
		}
	}

	private void UpdateAttachableInteraction() {
		if (_attachable == null) {
			return;
		}

		if (_attachable.OfAttachableType() == IAttachableEntity.AttachableEntityType.Weapon) {
			if (Input.Pressed("attack1")) {
				var weapon = _attachable.GetGameObject().GetComponent<IWeapon>();
				weapon.Shoot();
			}
		}
	}

	private void RenderArmsOnEyeLevel()
	{
		Rotation eyesRotation = Player.EyeAngles;
		Arms.LocalRotation = eyesRotation;
		Arms.WorldPosition = Player.EyePosition + eyesRotation.Forward * 15f + eyesRotation.Right * 5f;
	}
}
