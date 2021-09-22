#if NET35_OR_NEWER && !(NETSTANDARD || NET45_OR_NEWER)
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
        public static IEnumerable<Attribute> GetCustomAttributes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return assembly.GetCustomAttributes(false).Cast<Attribute>();
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
        public static IEnumerable<Attribute> GetCustomAttributes(this Assembly assembly, Type attributeType)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return assembly.GetCustomAttributes(attributeType, true).Cast<Attribute>();
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
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Assembly assembly) where TAttribute: Attribute
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return assembly.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
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
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return member.GetCustomAttributes(true).Cast<Attribute>();
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
        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo member, Type attributeType)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return member.GetCustomAttributes(attributeType, true).Cast<Attribute>();
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
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MemberInfo member) where TAttribute: Attribute
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            return member.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
        }
    }
}
#endif