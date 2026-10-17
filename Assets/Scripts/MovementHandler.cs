using System.Collections.Generic;

public class OneHiveRuleEnforcer {
    private bool _ruleSatisfied = false;
    public bool IsRuleSatisfied() { return _ruleSatisfied; }

    //TODO: this ignores rings
    public void RecalculateRule(Piece[] neighbors)
    {
        byte state = 0;
        for (byte i = 0; i < 6; i++)
        {
            bool isPresent = neighbors[i] != null && neighbors[i].GetCoords().z != 0;
            switch (state)
            {
                case 0: state = isPresent ? (byte)4 : (byte)1; break;
                case 1: state = isPresent ? (byte)2 : (byte)1; break;
                case 2: state = isPresent ? (byte)2 : (byte)3; break;
                case 3: state = isPresent ? (byte)7 : (byte)3; break;
                case 4: state = isPresent ? (byte)4 : (byte)5; break;
                case 5: state = isPresent ? (byte)6 : (byte)5; break;
                case 6: state = isPresent ? (byte)1 : (byte)7; break;
                default: break;
            }
        }
        _ruleSatisfied = state != 7;
    }
}

public class FreedomToMoveRuleEnforcer {
    private bool[] _ruleSatisfied = {false, false, false, false, false, false};
    public bool IsRuleSatisfied(int i) { return _ruleSatisfied[i]; }
    public void RecalculateRule(Piece[] neighbors)
    {
        for (int i = 0; i < 6; i++)
        {
            if (neighbors[i] != null && neighbors[i].GetCoords().z != 0)
            {
                _ruleSatisfied[i] = false; continue;
            } 
            int idxl = (i + 5) % 6;
            int idxr = (i + 1) % 6;
            _ruleSatisfied[i] =
                    neighbors[idxl] == null ||
                    neighbors[idxr] == null ||
                    neighbors[idxl].GetCoords().z == 0 ||
                    neighbors[idxr].GetCoords().z == 0;
        }
    }
}

public abstract class MovementHandler {}

public sealed class QueenMovementHandler : MovementHandler
{
    private OneHiveRuleEnforcer _oneHiveRuleEnforcer = new OneHiveRuleEnforcer();
    private FreedomToMoveRuleEnforcer _freedomToMoveRuleEnforcer = new FreedomToMoveRuleEnforcer();

    private List<MovementMove> _moves;

    public List<MovementMove> GetMoves()
    {
        return _moves;
    }

    //Alla fine questo metodo dovrà avere gli stessi parametri per tutti i pezzi,
    //in teoria una struct con tutte le info potenzialmente utili aggiornate regolarmente
    public void RecalculateMoves(Piece[] neighbors, byte index)
    {
        var newMoves = new List<MovementMove>();
        _oneHiveRuleEnforcer.RecalculateRule(neighbors);
        _freedomToMoveRuleEnforcer.RecalculateRule(neighbors);

        if (!_oneHiveRuleEnforcer.IsRuleSatisfied()) _moves = newMoves;

        for (int i = 0; i < 6; i++)
        {
            if (_freedomToMoveRuleEnforcer.IsRuleSatisfied(i))
            {
                MovementMove newMove = new MovementMove(index, neighbors[i].GetIndex(), false);

                newMoves.Add(newMove);
            }
        }

        _moves = newMoves;
    }
}