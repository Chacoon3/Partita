using JFrame.Core;

public class GameManager : ManagerBase {
    public static GameManager Instance = null;

    void Awake() {
        Instance = this;
    }
}
