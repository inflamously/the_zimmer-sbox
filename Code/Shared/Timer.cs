using System;

public class Timer : ITimeable
{
    bool _isEnabled;
    float _count;

    public Action _action;

	public float GetCount()
	{
        return _count;
	}

	public void Start(float time)
	{
        _count = time;
        _isEnabled = true;
	}

	public void Stop()
	{
        _count = 0;
        _isEnabled = false;
	}

    public void Pause() {
        _isEnabled = false;
    }

    public void Unpause() {
        _isEnabled = true;
    }

	public void Update( float deltaTime )
	{
        if (!_isEnabled) {
            return;
        }

        _count -= deltaTime;

        if (_count <= 0f) {
            _count = 0;

            if (_action != null) {
                _action.Invoke();
            }
        }
	}

    public bool IsActive() {
        return _count > 0f;
    }
}

public interface ITimeable
{
    void Start(float time);
    void Stop();
    float GetCount();
    bool IsActive();
    void Update(float deltaTime);
}