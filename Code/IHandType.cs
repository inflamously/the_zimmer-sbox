public enum SimulatorHandType {
	None, Left, Right
}

public interface IHandType {
    bool IsRightHand();
	bool IsLeftHand();
}