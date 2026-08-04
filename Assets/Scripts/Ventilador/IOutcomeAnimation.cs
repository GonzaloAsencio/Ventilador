namespace Fan.Decisions
{
    /// <summary>
    /// Contrato mínimo para la animación de consecuencia de una decisión.
    /// Permite que DecisionOutcomeSequencer dispare la animación sin
    /// conocer si es un movimiento, una rotación, o el día de mañana un Animator real.
    /// </summary>
    public interface IOutcomeAnimation
    {
        void Play();
    }
}