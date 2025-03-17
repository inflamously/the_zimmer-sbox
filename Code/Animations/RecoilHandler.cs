using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class RecoilHandler : Component, IAnimatable {
   [Property] float RecoilTime = 2f;
   [Property] public Rotation RecoilOffset;
   Rotation _recoilRotation = Rotation.Identity;

    CancellationTokenSource _cancelAnim;

   public async Task RecoilIntoPlace( CancellationToken token ) {
        var recoilTimer = 0f;
        while (recoilTimer < RecoilTime) {
            _recoilRotation = Rotation.Lerp(_recoilRotation, Rotation.Identity, recoilTimer / RecoilTime);
            recoilTimer += Time.Delta;
			if (token.IsCancellationRequested) {
                Log.Info("Cancelation requested");
				await Task.CompletedTask;
                return;
			}
            await Task.Frame();
        }
    }

    public Task Run() {
        if (_cancelAnim != null && !_cancelAnim.IsCancellationRequested) {
			_cancelAnim.Cancel();
		}
        _cancelAnim = new CancellationTokenSource();
        _recoilRotation = RecoilOffset;
        return RecoilIntoPlace(_cancelAnim.Token);
    }

	public Rotation GetRotationAnim()
	{
		return _recoilRotation;
	}

	public Vector3 GetPositionAnim()
	{
		throw new NotImplementedException();
	}
}