public class TrapHole : IEffect
{
	#region Card Effect Works Functions
	public override bool TargetCondition(Card target)
	{
		if (target is MonsterCard)
		{
			MonsterCard targetmonster = target as MonsterCard;
			if (targetmonster.atk < 1000)
			{
				return false;
			}
		}
		else
		{
			return false;
		}
		return true;
	}
	public override bool EffectCondition(Card card)
	{
		if (card.pos != CardPosition.MagicField) return false;
		if (card.setturn == -1 || card.playManager.GetTotalTurn() - card.setturn < 1)
		{

			return false;
		}
		if (card.owner == card.targetcard.owner)
		{

			return false;
		}
		if (card.targetcard is MonsterCard)
		{
			MonsterCard targetmonster = card.targetcard as MonsterCard;
			if (targetmonster.atk < 1000)
			{
				return false;
			}
		}
		else
		{
			return false;
		}
		return true;
	}
	public override void Effect(Card card)
	{
		card.playManager.DestroyCard(card.targetcard, DeathReason.EffectDestroy);
	}
	public override Card AutoTargetFunction(Card card)
	{
		return card.targetcard;
	}
	public override void RemovePassiveEffect(Card card)
	{
		return;
	}
	#endregion
}
