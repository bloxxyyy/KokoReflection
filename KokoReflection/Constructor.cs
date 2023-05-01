using System.Reflection;

namespace KokoReflection;

public static class Constructor {
    public static ConstructorInfo? GetExactConstructor<T>(Type[] parameterTypes) {

        var exactConstructor = typeof(T).GetConstructor(parameterTypes);
        if (exactConstructor != null) return exactConstructor;

        // If we do have null values. We need to find the constructor with the most matching parameters.
        var compatibleConstructors = typeof(T).GetConstructors().Where(constructor => {
            var constructorParams = constructor.GetParameters();

            // If the constructor has more or less parameters than we have arguments, it is not compatible.
            if (constructorParams.Length != parameterTypes.Length)
                return false;

            for (int i = 0; i < constructorParams.Length; i++) {
                // object is not null so just go to the next parameter.
                if (parameterTypes[i] == typeof(object)) {
                    continue;
                }

                // this is the important part. Check if the parameter type is compatible with the argument type.
                if (!constructorParams[i].ParameterType.IsAssignableFrom(parameterTypes[i])) {
                    return false;
                }
            }

            // If we got here, all parameters are compatible.
            return true;
        });

        return compatibleConstructors.FirstOrDefault();
    }
}
