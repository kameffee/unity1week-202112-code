using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity1week202112.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.Status
{
    public class PlayerHpView : MonoBehaviour
    {
        [SerializeField]
        private Slider _hpSlider;

        [SerializeField]
        private Slider _hpSubSlider;

        [SerializeField]
        private TextMeshProUGUI _max;

        [SerializeField]
        private TextMeshProUGUI _value;

        [SerializeField]
        private RectTransform _shakeTargetRoot;

        [Header("Damage Effeect")]
        [SerializeField]
        private Transform _effectHolder;

        [SerializeField]
        private DamagePointView _damageEffectPrefab;

        [SerializeField]
        private HealPointView _healEffectPrefab;

        private int _hp;

        public void SetMaxHp(int maxHp)
        {
            _hpSlider.maxValue = maxHp;
            _hpSubSlider.maxValue = maxHp;
            _max.SetText(maxHp.ToString());
        }

        public void RenderHp(int hp)
        {
            _hp = hp;
            _hpSlider.value = hp;
            _hpSubSlider.value = hp;
            _value.SetText(hp.ToString());
        }

        public async void Damege(HpDamage hpDamage)
        {
            // 揺らす
            await _shakeTargetRoot.DOShakeAnchorPos(0.5f, 10f, 20);

            Sequence sequence = DOTween.Sequence();

            sequence.Append(_hpSlider.DOValue(hpDamage.ToHp, 0.5f).SetEase(Ease.OutSine));
            sequence.Join(_value.DOCounter(_hp, hpDamage.ToHp, 0.5f).SetEase(Ease.OutSine));
            sequence.AppendInterval(0.5f);
            sequence.Append(_hpSubSlider.DOValue(hpDamage.ToHp, 0.5f).SetEase(Ease.OutSine));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());

            // ダメージ量エフェクト
            DamagePointView effect = Instantiate(_damageEffectPrefab, _effectHolder);
            effect.SetDamage(hpDamage.Damage);
            effect.PlayAnimation();

            _hp = hpDamage.ToHp;
        }

        public void Cure(HpHeal hpHeal)
        {
            _hpSlider.DOValue(hpHeal.AfterHp, 0.5f)
                .SetEase(Ease.OutSine)
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            _hpSubSlider.DOValue(hpHeal.AfterHp, 0.5f)
                .SetEase(Ease.OutSine)
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            _value.DOCounter(_hp, hpHeal.AfterHp, 0.5f)
                .SetEase(Ease.OutSine)
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            // 回復エフェクト
            var effect = Instantiate(_healEffectPrefab, _effectHolder);
            effect.SetValue(hpHeal.Heal);
            effect.PlayAnimation();

            _hp = hpHeal.AfterHp;
        }
    }
}
