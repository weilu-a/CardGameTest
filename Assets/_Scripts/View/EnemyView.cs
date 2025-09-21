using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;
    public int AttackPower { get; set; }

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        UpdateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);

        if (enemyData.AnimatorController != null && animator != null)
        {
            animator.runtimeAnimatorController = enemyData.AnimatorController;
        }
    }

    private void UpdateAttackText()
    {
        attackText.text = "ATK:" + AttackPower;
    }
}
