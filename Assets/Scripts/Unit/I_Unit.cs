public interface I_Unit
{
    bool CanMove();
    bool CanMoveTo(E_Direction _Direction);
    E_TileAction Move(E_Direction _Direction);

    void TakeDamages(int _Damages);
    void Heal(int _Heal);

    int Health { get; }
    int MaxHealth { get; }

    void RegisterHealthObserver(I_UnitHealthObserver _Observer);
    void UnregisterHealthObserver(I_UnitHealthObserver _Observer);

    void Kill();
}
