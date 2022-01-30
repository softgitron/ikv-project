using Newtonsoft.Json;

public class StateSyncImpl<T> : TCPAction
{

    private StateSync<T> controllObject;
    private T state;

    private string handleName;

    public StateSyncImpl(StateSync<T> controllObject, T state, string handleName) {
        this.controllObject = controllObject;
        this.state = state;
        this.handleName = handleName;
        initialize();
    }

    private void initialize() {
        TCPImpl.RegisterListener(handleName, this);
    }

    public T GetState() {
        return state;
    }

    public void SetState(T newState)
    {
        state = newState;
        string jsonCommand = jsonCommand = JsonConvert.SerializeObject(state);
        TCPImpl.SendCommand(handleName, jsonCommand);
    }

    public void TCPAction(string parameters) {
        if (controllObject == null) {
            return;
        }
        
        T jsonFields = JsonConvert.DeserializeObject<T>(parameters);
        state = jsonFields;
        controllObject.StateTrigger(jsonFields);
    }
}
