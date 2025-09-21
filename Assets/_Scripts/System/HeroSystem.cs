using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field:SerializeField] public HeroView HeroView { get; private set; }

    [SerializeField] private float attackMoveDuration = 0.3f;
    [SerializeField] private float returnMoveDuration = 0.3f;

    private Vector3 originalPosition;
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AttackEnemyGA>(AttackEnemyPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackEnemyGA>();


    }

    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
        originalPosition = HeroView.transform.position;
    }

    private IEnumerator AttackEnemyPerformer(AttackEnemyGA attackEnemyGA)
    {
        HeroView.animator.SetTrigger("isAttacking");

        Vector3 attackPosition = attackEnemyGA.Targets.transform.position - Vector3.right * 3;

        yield return new WaitForSeconds(0.1f);

        HeroView.transform.DOMove(attackPosition, attackMoveDuration);
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(0.6f);

        HeroView.transform.DOMove(originalPosition, returnMoveDuration);
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(0.1f);
    }
}
