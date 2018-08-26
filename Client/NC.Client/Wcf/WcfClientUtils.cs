using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace NC.Client.Wcf
{
    /// <summary>
    /// <see cref="WcfClient{TContract}"/> utilities.
    /// </summary>
    public static class WcfClientUtils
    {
        /// <summary>
        /// Получить привязку размещенную в конфиге исполняемого файла.
        /// </summary>
        /// <param name="name">Название привязки.</param>
        /// <returns>Экземпляр привязки.</returns>
        public static Binding ResolveBinding(string name)
        {
            var config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var section = GetBindingsSection(config);

            foreach (var bindingCollection in section.BindingCollections)
            {
                var bindingElement = bindingCollection.ConfiguredBindings.FirstOrDefault(
                    binding => binding.Name == name);
                if (bindingElement != null)
                {
                    var binding = (Binding)Activator.CreateInstance(bindingCollection.BindingType);
                    binding.Name = bindingElement.Name;
                    bindingElement.ApplyConfiguration(binding);

                    return binding;
                }
            }

            return null;
        }

        private static BindingsSection GetBindingsSection(string path)
        {
            var config =
                ConfigurationManager.OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap { ExeConfigFilename = path },
                    ConfigurationUserLevel.None);

            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(config);

            return serviceModel.Bindings;
        }
    }
}