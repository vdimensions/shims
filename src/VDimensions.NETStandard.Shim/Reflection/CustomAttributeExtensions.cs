#if FX_CUSTOM_ATTRIBUTES
using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// A static class to provide the functionality of `GetCustomAttributes` method.
    /// </summary>
    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="Assembly">assembly</see>.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly">assembly</see> to get custom attributes for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="assembly" />,
        /// or an empty collection if no such attributes exist.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// A custom attribute type could not be loaded.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="assembly" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<Attribute> GetCustomAttributes(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return Enumerable.Cast<Attribute>(assembly.GetCustomAttributes(false));
        }
        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="Assembly">assembly</see>.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly">assembly</see> to get custom attributes for.
        /// </param>
        /// <param name="attributeType">
        /// The <see cref="Type">type</see> of attribute to search for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="assembly" />,
        /// or an empty collection if no such attributes exist. 
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// A custom attribute type could not be loaded. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="assembly" /> or <paramref name="attributeType" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<Attribute> GetCustomAttributes(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Assembly assembly, Type attributeType)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return Enumerable.Cast<Attribute>(assembly.GetCustomAttributes(attributeType, true));
        }
        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="Assembly">assembly</see>.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// The <see cref="Type">type</see> of attribute to search for.
        /// </typeparam>
        /// <param name="assembly">
        /// The <see cref="Assembly">assembly</see> to get custom attributes for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="assembly" />,
        /// or an empty collection if no such attributes exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="assembly" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Assembly assembly) where TAttribute: Attribute
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return Enumerable.Cast<TAttribute>(assembly.GetCustomAttributes(typeof(TAttribute), true));
        }

        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="MemberInfo">member</see>.
        /// </summary>
        /// <param name="member">
        /// The <see cref="MemberInfo">member</see> to get custom attributes for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="member" />,
        /// or an empty collection if no such attributes exist.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// A custom attribute type could not be loaded.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="member" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<Attribute> GetCustomAttributes(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return Enumerable.Cast<Attribute>(member.GetCustomAttributes(true));
        }
        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="MemberInfo">member</see>.
        /// </summary>
        /// <param name="member">
        /// The <see cref="MemberInfo">member</see> to get custom attributes for.
        /// </param>
        /// <param name="attributeType">
        /// The <see cref="Type">type</see> of attribute to search for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="member" />,
        /// or an empty collection if no such attributes exist.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// A custom attribute type could not be loaded.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="member" /> or <paramref name="attributeType" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<Attribute> GetCustomAttributes(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            MemberInfo member, Type attributeType)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return Enumerable.Cast<Attribute>(member.GetCustomAttributes(attributeType, true));
        }
        /// <summary>
        /// Retrieves a collection of custom attributes that are applied to a specified <see cref="MemberInfo">member</see>.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// The <see cref="Type">type</see> of attribute to search for.
        /// </typeparam>
        /// <param name="member">
        /// The <see cref="MemberInfo">member</see> to get custom attributes for.
        /// </param>
        /// <returns>
        /// A collection of the custom attributes that are applied to <paramref name="member" />,
        /// or an empty collection if no such attributes exist. 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="member" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            MemberInfo member) where TAttribute: Attribute
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return Enumerable.Cast<TAttribute>(member.GetCustomAttributes(typeof(TAttribute), true));
        }
    }
}
#endif