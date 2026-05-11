namespace SmartHomeApp.Devices
{
    // Definisi State untuk Automata
    public enum AlarmState { Disarmed, ArmedAway, Triggered }

    public class SecurityAlarm
    {
        public AlarmState CurrentState { get; private set; } = AlarmState.Disarmed;

        // Implementasi Automata & Design by Contract (DbC)
        public void ChangeState(AlarmState newState, string pin)
        {
            [cite_start]// Pre-condition (DbC): Validasi PIN tidak boleh kosong [cite: 12]
            if (string.IsNullOrEmpty(pin) || pin.Length < 4)
            {
                throw new ArgumentException("PIN harus minimal 4 digit!");
            }

            // Logika Transisi State
            if (CurrentState == AlarmState.Disarmed && newState == AlarmState.ArmedAway)
            {
                CurrentState = newState;
            }
            else if (newState == AlarmState.Triggered)
            {
                CurrentState = newState;
            }
        }
    }
}