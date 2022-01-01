using Unity1week202112.Presentation;
using Unity1week202112.Presentation.Title;
using Unity1week202112.Presentation.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202112.Installer
{
    public class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SettingsWindow _settingsWindowPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<TitleView>();
            builder.RegisterComponentInHierarchy<TextShake>();
            builder.RegisterEntryPoint<TitleScene>();

            builder.RegisterComponentInNewPrefab(_settingsWindowPrefab, Lifetime.Singleton);
            builder.RegisterFactory<SettingsWindowPresenter>(
                resolver => () => new SettingsWindowPresenter(Instantiate(_settingsWindowPrefab)),
                Lifetime.Singleton);
        }
    }
}
