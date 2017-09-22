﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnSpy.Debugger.DotNet.Metadata.Impl {
	sealed partial class WellKnownMemberResolver {
		struct TypeName {
			public readonly string Namespace;
			public readonly string Name;
			public TypeName(string @namespace, string name) {
				Namespace = @namespace ?? string.Empty;
				Name = name;
			}
		}

		sealed class TypeNameEqualityComparer : IEqualityComparer<TypeName> {
			public static readonly TypeNameEqualityComparer Instance = new TypeNameEqualityComparer();
			TypeNameEqualityComparer() { }
			public bool Equals(TypeName x, TypeName y) => StringComparer.Ordinal.Equals(x.Name, y.Name) && StringComparer.Ordinal.Equals(x.Namespace, y.Namespace);
			public int GetHashCode(TypeName obj) => StringComparer.Ordinal.GetHashCode(obj.Namespace) ^ StringComparer.Ordinal.GetHashCode(obj.Name);
		}

		const int WELL_KNOWN_NONNESTED_TYPES_COUNT = 275;
		const int WELL_KNOWN_NESTED_TYPES_COUNT = 3;
		const int WELL_KNOWN_TYPES_COUNT = WELL_KNOWN_NONNESTED_TYPES_COUNT + WELL_KNOWN_NESTED_TYPES_COUNT;
		static readonly Dictionary<TypeName, DmdWellKnownType> toNonNestedWellKnownType = new Dictionary<TypeName, DmdWellKnownType>(WELL_KNOWN_NONNESTED_TYPES_COUNT, TypeNameEqualityComparer.Instance) {
			{ new TypeName("System", "Object"), DmdWellKnownType.System_Object },
			{ new TypeName("System", "Enum"), DmdWellKnownType.System_Enum },
			{ new TypeName("System", "MulticastDelegate"), DmdWellKnownType.System_MulticastDelegate },
			{ new TypeName("System", "Delegate"), DmdWellKnownType.System_Delegate },
			{ new TypeName("System", "ValueType"), DmdWellKnownType.System_ValueType },
			{ new TypeName("System", "Void"), DmdWellKnownType.System_Void },
			{ new TypeName("System", "Boolean"), DmdWellKnownType.System_Boolean },
			{ new TypeName("System", "Char"), DmdWellKnownType.System_Char },
			{ new TypeName("System", "SByte"), DmdWellKnownType.System_SByte },
			{ new TypeName("System", "Byte"), DmdWellKnownType.System_Byte },
			{ new TypeName("System", "Int16"), DmdWellKnownType.System_Int16 },
			{ new TypeName("System", "UInt16"), DmdWellKnownType.System_UInt16 },
			{ new TypeName("System", "Int32"), DmdWellKnownType.System_Int32 },
			{ new TypeName("System", "UInt32"), DmdWellKnownType.System_UInt32 },
			{ new TypeName("System", "Int64"), DmdWellKnownType.System_Int64 },
			{ new TypeName("System", "UInt64"), DmdWellKnownType.System_UInt64 },
			{ new TypeName("System", "Decimal"), DmdWellKnownType.System_Decimal },
			{ new TypeName("System", "Single"), DmdWellKnownType.System_Single },
			{ new TypeName("System", "Double"), DmdWellKnownType.System_Double },
			{ new TypeName("System", "String"), DmdWellKnownType.System_String },
			{ new TypeName("System", "IntPtr"), DmdWellKnownType.System_IntPtr },
			{ new TypeName("System", "UIntPtr"), DmdWellKnownType.System_UIntPtr },
			{ new TypeName("System", "Array"), DmdWellKnownType.System_Array },
			{ new TypeName("System.Collections", "IEnumerable"), DmdWellKnownType.System_Collections_IEnumerable },
			{ new TypeName("System.Collections.Generic", "IEnumerable`1"), DmdWellKnownType.System_Collections_Generic_IEnumerable_T },
			{ new TypeName("System.Collections.Generic", "IList`1"), DmdWellKnownType.System_Collections_Generic_IList_T },
			{ new TypeName("System.Collections.Generic", "ICollection`1"), DmdWellKnownType.System_Collections_Generic_ICollection_T },
			{ new TypeName("System.Collections", "IEnumerator"), DmdWellKnownType.System_Collections_IEnumerator },
			{ new TypeName("System.Collections.Generic", "IEnumerator`1"), DmdWellKnownType.System_Collections_Generic_IEnumerator_T },
			{ new TypeName("System.Collections.Generic", "IReadOnlyList`1"), DmdWellKnownType.System_Collections_Generic_IReadOnlyList_T },
			{ new TypeName("System.Collections.Generic", "IReadOnlyCollection`1"), DmdWellKnownType.System_Collections_Generic_IReadOnlyCollection_T },
			{ new TypeName("System", "Nullable`1"), DmdWellKnownType.System_Nullable_T },
			{ new TypeName("System", "DateTime"), DmdWellKnownType.System_DateTime },
			{ new TypeName("System.Runtime.CompilerServices", "IsVolatile"), DmdWellKnownType.System_Runtime_CompilerServices_IsVolatile },
			{ new TypeName("System", "IDisposable"), DmdWellKnownType.System_IDisposable },
			{ new TypeName("System", "TypedReference"), DmdWellKnownType.System_TypedReference },
			{ new TypeName("System", "ArgIterator"), DmdWellKnownType.System_ArgIterator },
			{ new TypeName("System", "RuntimeArgumentHandle"), DmdWellKnownType.System_RuntimeArgumentHandle },
			{ new TypeName("System", "RuntimeFieldHandle"), DmdWellKnownType.System_RuntimeFieldHandle },
			{ new TypeName("System", "RuntimeMethodHandle"), DmdWellKnownType.System_RuntimeMethodHandle },
			{ new TypeName("System", "RuntimeTypeHandle"), DmdWellKnownType.System_RuntimeTypeHandle },
			{ new TypeName("System", "IAsyncResult"), DmdWellKnownType.System_IAsyncResult },
			{ new TypeName("System", "AsyncCallback"), DmdWellKnownType.System_AsyncCallback },
			{ new TypeName("System", "Math"), DmdWellKnownType.System_Math },
			{ new TypeName("System", "Attribute"), DmdWellKnownType.System_Attribute },
			{ new TypeName("System", "CLSCompliantAttribute"), DmdWellKnownType.System_CLSCompliantAttribute },
			{ new TypeName("System", "Convert"), DmdWellKnownType.System_Convert },
			{ new TypeName("System", "Exception"), DmdWellKnownType.System_Exception },
			{ new TypeName("System", "FlagsAttribute"), DmdWellKnownType.System_FlagsAttribute },
			{ new TypeName("System", "FormattableString"), DmdWellKnownType.System_FormattableString },
			{ new TypeName("System", "Guid"), DmdWellKnownType.System_Guid },
			{ new TypeName("System", "IFormattable"), DmdWellKnownType.System_IFormattable },
			{ new TypeName("System", "MarshalByRefObject"), DmdWellKnownType.System_MarshalByRefObject },
			{ new TypeName("System", "Type"), DmdWellKnownType.System_Type },
			{ new TypeName("System.Reflection", "AssemblyKeyFileAttribute"), DmdWellKnownType.System_Reflection_AssemblyKeyFileAttribute },
			{ new TypeName("System.Reflection", "AssemblyKeyNameAttribute"), DmdWellKnownType.System_Reflection_AssemblyKeyNameAttribute },
			{ new TypeName("System.Reflection", "MethodInfo"), DmdWellKnownType.System_Reflection_MethodInfo },
			{ new TypeName("System.Reflection", "ConstructorInfo"), DmdWellKnownType.System_Reflection_ConstructorInfo },
			{ new TypeName("System.Reflection", "MethodBase"), DmdWellKnownType.System_Reflection_MethodBase },
			{ new TypeName("System.Reflection", "FieldInfo"), DmdWellKnownType.System_Reflection_FieldInfo },
			{ new TypeName("System.Reflection", "MemberInfo"), DmdWellKnownType.System_Reflection_MemberInfo },
			{ new TypeName("System.Reflection", "Missing"), DmdWellKnownType.System_Reflection_Missing },
			{ new TypeName("System.Runtime.CompilerServices", "FormattableStringFactory"), DmdWellKnownType.System_Runtime_CompilerServices_FormattableStringFactory },
			{ new TypeName("System.Runtime.CompilerServices", "RuntimeHelpers"), DmdWellKnownType.System_Runtime_CompilerServices_RuntimeHelpers },
			{ new TypeName("System.Runtime.ExceptionServices", "ExceptionDispatchInfo"), DmdWellKnownType.System_Runtime_ExceptionServices_ExceptionDispatchInfo },
			{ new TypeName("System.Runtime.InteropServices", "StructLayoutAttribute"), DmdWellKnownType.System_Runtime_InteropServices_StructLayoutAttribute },
			{ new TypeName("System.Runtime.InteropServices", "UnknownWrapper"), DmdWellKnownType.System_Runtime_InteropServices_UnknownWrapper },
			{ new TypeName("System.Runtime.InteropServices", "DispatchWrapper"), DmdWellKnownType.System_Runtime_InteropServices_DispatchWrapper },
			{ new TypeName("System.Runtime.InteropServices", "CallingConvention"), DmdWellKnownType.System_Runtime_InteropServices_CallingConvention },
			{ new TypeName("System.Runtime.InteropServices", "ClassInterfaceAttribute"), DmdWellKnownType.System_Runtime_InteropServices_ClassInterfaceAttribute },
			{ new TypeName("System.Runtime.InteropServices", "ClassInterfaceType"), DmdWellKnownType.System_Runtime_InteropServices_ClassInterfaceType },
			{ new TypeName("System.Runtime.InteropServices", "CoClassAttribute"), DmdWellKnownType.System_Runtime_InteropServices_CoClassAttribute },
			{ new TypeName("System.Runtime.InteropServices", "ComAwareEventInfo"), DmdWellKnownType.System_Runtime_InteropServices_ComAwareEventInfo },
			{ new TypeName("System.Runtime.InteropServices", "ComEventInterfaceAttribute"), DmdWellKnownType.System_Runtime_InteropServices_ComEventInterfaceAttribute },
			{ new TypeName("System.Runtime.InteropServices", "ComInterfaceType"), DmdWellKnownType.System_Runtime_InteropServices_ComInterfaceType },
			{ new TypeName("System.Runtime.InteropServices", "ComSourceInterfacesAttribute"), DmdWellKnownType.System_Runtime_InteropServices_ComSourceInterfacesAttribute },
			{ new TypeName("System.Runtime.InteropServices", "ComVisibleAttribute"), DmdWellKnownType.System_Runtime_InteropServices_ComVisibleAttribute },
			{ new TypeName("System.Runtime.InteropServices", "DispIdAttribute"), DmdWellKnownType.System_Runtime_InteropServices_DispIdAttribute },
			{ new TypeName("System.Runtime.InteropServices", "GuidAttribute"), DmdWellKnownType.System_Runtime_InteropServices_GuidAttribute },
			{ new TypeName("System.Runtime.InteropServices", "InterfaceTypeAttribute"), DmdWellKnownType.System_Runtime_InteropServices_InterfaceTypeAttribute },
			{ new TypeName("System.Runtime.InteropServices", "Marshal"), DmdWellKnownType.System_Runtime_InteropServices_Marshal },
			{ new TypeName("System.Runtime.InteropServices", "TypeIdentifierAttribute"), DmdWellKnownType.System_Runtime_InteropServices_TypeIdentifierAttribute },
			{ new TypeName("System.Runtime.InteropServices", "BestFitMappingAttribute"), DmdWellKnownType.System_Runtime_InteropServices_BestFitMappingAttribute },
			{ new TypeName("System.Runtime.InteropServices", "DefaultParameterValueAttribute"), DmdWellKnownType.System_Runtime_InteropServices_DefaultParameterValueAttribute },
			{ new TypeName("System.Runtime.InteropServices", "LCIDConversionAttribute"), DmdWellKnownType.System_Runtime_InteropServices_LCIDConversionAttribute },
			{ new TypeName("System.Runtime.InteropServices", "UnmanagedFunctionPointerAttribute"), DmdWellKnownType.System_Runtime_InteropServices_UnmanagedFunctionPointerAttribute },
			{ new TypeName("System", "Activator"), DmdWellKnownType.System_Activator },
			{ new TypeName("System.Threading.Tasks", "Task"), DmdWellKnownType.System_Threading_Tasks_Task },
			{ new TypeName("System.Threading.Tasks", "Task`1"), DmdWellKnownType.System_Threading_Tasks_Task_T },
			{ new TypeName("System.Threading", "Interlocked"), DmdWellKnownType.System_Threading_Interlocked },
			{ new TypeName("System.Threading", "Monitor"), DmdWellKnownType.System_Threading_Monitor },
			{ new TypeName("System.Threading", "Thread"), DmdWellKnownType.System_Threading_Thread },
			{ new TypeName("Microsoft.CSharp.RuntimeBinder", "Binder"), DmdWellKnownType.Microsoft_CSharp_RuntimeBinder_Binder },
			{ new TypeName("Microsoft.CSharp.RuntimeBinder", "CSharpArgumentInfo"), DmdWellKnownType.Microsoft_CSharp_RuntimeBinder_CSharpArgumentInfo },
			{ new TypeName("Microsoft.CSharp.RuntimeBinder", "CSharpArgumentInfoFlags"), DmdWellKnownType.Microsoft_CSharp_RuntimeBinder_CSharpArgumentInfoFlags },
			{ new TypeName("Microsoft.CSharp.RuntimeBinder", "CSharpBinderFlags"), DmdWellKnownType.Microsoft_CSharp_RuntimeBinder_CSharpBinderFlags },
			{ new TypeName("Microsoft.VisualBasic", "CallType"), DmdWellKnownType.Microsoft_VisualBasic_CallType },
			{ new TypeName("Microsoft.VisualBasic", "Embedded"), DmdWellKnownType.Microsoft_VisualBasic_Embedded },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "Conversions"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_Conversions },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "Operators"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_Operators },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "NewLateBinding"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_NewLateBinding },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "EmbeddedOperators"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_EmbeddedOperators },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "StandardModuleAttribute"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_StandardModuleAttribute },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "Utils"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_Utils },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "LikeOperator"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_LikeOperator },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "ProjectData"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_ProjectData },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "ObjectFlowControl"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_ObjectFlowControl },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "StaticLocalInitFlag"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_StaticLocalInitFlag },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "StringType"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_StringType },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "IncompleteInitialization"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_IncompleteInitialization },
			{ new TypeName("Microsoft.VisualBasic.CompilerServices", "Versioned"), DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_Versioned },
			{ new TypeName("Microsoft.VisualBasic", "CompareMethod"), DmdWellKnownType.Microsoft_VisualBasic_CompareMethod },
			{ new TypeName("Microsoft.VisualBasic", "Strings"), DmdWellKnownType.Microsoft_VisualBasic_Strings },
			{ new TypeName("Microsoft.VisualBasic", "ErrObject"), DmdWellKnownType.Microsoft_VisualBasic_ErrObject },
			{ new TypeName("Microsoft.VisualBasic", "FileSystem"), DmdWellKnownType.Microsoft_VisualBasic_FileSystem },
			{ new TypeName("Microsoft.VisualBasic.ApplicationServices", "ApplicationBase"), DmdWellKnownType.Microsoft_VisualBasic_ApplicationServices_ApplicationBase },
			{ new TypeName("Microsoft.VisualBasic.ApplicationServices", "WindowsFormsApplicationBase"), DmdWellKnownType.Microsoft_VisualBasic_ApplicationServices_WindowsFormsApplicationBase },
			{ new TypeName("Microsoft.VisualBasic", "Information"), DmdWellKnownType.Microsoft_VisualBasic_Information },
			{ new TypeName("Microsoft.VisualBasic", "Interaction"), DmdWellKnownType.Microsoft_VisualBasic_Interaction },
			{ new TypeName("System", "Func`1"), DmdWellKnownType.System_Func_T },
			{ new TypeName("System", "Func`2"), DmdWellKnownType.System_Func_T2 },
			{ new TypeName("System", "Func`3"), DmdWellKnownType.System_Func_T3 },
			{ new TypeName("System", "Func`4"), DmdWellKnownType.System_Func_T4 },
			{ new TypeName("System", "Func`5"), DmdWellKnownType.System_Func_T5 },
			{ new TypeName("System", "Func`6"), DmdWellKnownType.System_Func_T6 },
			{ new TypeName("System", "Func`7"), DmdWellKnownType.System_Func_T7 },
			{ new TypeName("System", "Func`8"), DmdWellKnownType.System_Func_T8 },
			{ new TypeName("System", "Func`9"), DmdWellKnownType.System_Func_T9 },
			{ new TypeName("System", "Func`10"), DmdWellKnownType.System_Func_T10 },
			{ new TypeName("System", "Func`11"), DmdWellKnownType.System_Func_T11 },
			{ new TypeName("System", "Func`12"), DmdWellKnownType.System_Func_T12 },
			{ new TypeName("System", "Func`13"), DmdWellKnownType.System_Func_T13 },
			{ new TypeName("System", "Func`14"), DmdWellKnownType.System_Func_T14 },
			{ new TypeName("System", "Func`15"), DmdWellKnownType.System_Func_T15 },
			{ new TypeName("System", "Func`16"), DmdWellKnownType.System_Func_T16 },
			{ new TypeName("System", "Func`17"), DmdWellKnownType.System_Func_T17 },
			{ new TypeName("System", "Action"), DmdWellKnownType.System_Action },
			{ new TypeName("System", "Action`1"), DmdWellKnownType.System_Action_T },
			{ new TypeName("System", "Action`2"), DmdWellKnownType.System_Action_T2 },
			{ new TypeName("System", "Action`3"), DmdWellKnownType.System_Action_T3 },
			{ new TypeName("System", "Action`4"), DmdWellKnownType.System_Action_T4 },
			{ new TypeName("System", "Action`5"), DmdWellKnownType.System_Action_T5 },
			{ new TypeName("System", "Action`6"), DmdWellKnownType.System_Action_T6 },
			{ new TypeName("System", "Action`7"), DmdWellKnownType.System_Action_T7 },
			{ new TypeName("System", "Action`8"), DmdWellKnownType.System_Action_T8 },
			{ new TypeName("System", "Action`9"), DmdWellKnownType.System_Action_T9 },
			{ new TypeName("System", "Action`10"), DmdWellKnownType.System_Action_T10 },
			{ new TypeName("System", "Action`11"), DmdWellKnownType.System_Action_T11 },
			{ new TypeName("System", "Action`12"), DmdWellKnownType.System_Action_T12 },
			{ new TypeName("System", "Action`13"), DmdWellKnownType.System_Action_T13 },
			{ new TypeName("System", "Action`14"), DmdWellKnownType.System_Action_T14 },
			{ new TypeName("System", "Action`15"), DmdWellKnownType.System_Action_T15 },
			{ new TypeName("System", "Action`16"), DmdWellKnownType.System_Action_T16 },
			{ new TypeName("System", "AttributeUsageAttribute"), DmdWellKnownType.System_AttributeUsageAttribute },
			{ new TypeName("System", "ParamArrayAttribute"), DmdWellKnownType.System_ParamArrayAttribute },
			{ new TypeName("System", "NonSerializedAttribute"), DmdWellKnownType.System_NonSerializedAttribute },
			{ new TypeName("System", "STAThreadAttribute"), DmdWellKnownType.System_STAThreadAttribute },
			{ new TypeName("System.Reflection", "DefaultMemberAttribute"), DmdWellKnownType.System_Reflection_DefaultMemberAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "DateTimeConstantAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_DateTimeConstantAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "DecimalConstantAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_DecimalConstantAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "IUnknownConstantAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_IUnknownConstantAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "IDispatchConstantAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_IDispatchConstantAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "ExtensionAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_ExtensionAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "INotifyCompletion"), DmdWellKnownType.System_Runtime_CompilerServices_INotifyCompletion },
			{ new TypeName("System.Runtime.CompilerServices", "InternalsVisibleToAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_InternalsVisibleToAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "CompilerGeneratedAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_CompilerGeneratedAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "AccessedThroughPropertyAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_AccessedThroughPropertyAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "CompilationRelaxationsAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_CompilationRelaxationsAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "RuntimeCompatibilityAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_RuntimeCompatibilityAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "UnsafeValueTypeAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_UnsafeValueTypeAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "FixedBufferAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_FixedBufferAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "DynamicAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_DynamicAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "CallSiteBinder"), DmdWellKnownType.System_Runtime_CompilerServices_CallSiteBinder },
			{ new TypeName("System.Runtime.CompilerServices", "CallSite"), DmdWellKnownType.System_Runtime_CompilerServices_CallSite },
			{ new TypeName("System.Runtime.CompilerServices", "CallSite`1"), DmdWellKnownType.System_Runtime_CompilerServices_CallSite_T },
			{ new TypeName("System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationToken"), DmdWellKnownType.System_Runtime_InteropServices_WindowsRuntime_EventRegistrationToken },
			{ new TypeName("System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationTokenTable`1"), DmdWellKnownType.System_Runtime_InteropServices_WindowsRuntime_EventRegistrationTokenTable_T },
			{ new TypeName("System.Runtime.InteropServices.WindowsRuntime", "WindowsRuntimeMarshal"), DmdWellKnownType.System_Runtime_InteropServices_WindowsRuntime_WindowsRuntimeMarshal },
			{ new TypeName("Windows.Foundation", "IAsyncAction"), DmdWellKnownType.Windows_Foundation_IAsyncAction },
			{ new TypeName("Windows.Foundation", "IAsyncActionWithProgress`1"), DmdWellKnownType.Windows_Foundation_IAsyncActionWithProgress_T },
			{ new TypeName("Windows.Foundation", "IAsyncOperation`1"), DmdWellKnownType.Windows_Foundation_IAsyncOperation_T },
			{ new TypeName("Windows.Foundation", "IAsyncOperationWithProgress`2"), DmdWellKnownType.Windows_Foundation_IAsyncOperationWithProgress_T2 },
			{ new TypeName("System.Diagnostics", "Debugger"), DmdWellKnownType.System_Diagnostics_Debugger },
			{ new TypeName("System.Diagnostics", "DebuggerDisplayAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerDisplayAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerNonUserCodeAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerNonUserCodeAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerHiddenAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerHiddenAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerBrowsableAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerBrowsableAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerStepThroughAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerStepThroughAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerBrowsableState"), DmdWellKnownType.System_Diagnostics_DebuggerBrowsableState },
			{ new TypeName("System.Diagnostics", "DebuggableAttribute"), DmdWellKnownType.System_Diagnostics_DebuggableAttribute },
			{ new TypeName("System.ComponentModel", "DesignerSerializationVisibilityAttribute"), DmdWellKnownType.System_ComponentModel_DesignerSerializationVisibilityAttribute },
			{ new TypeName("System", "IEquatable`1"), DmdWellKnownType.System_IEquatable_T },
			{ new TypeName("System.Collections", "IList"), DmdWellKnownType.System_Collections_IList },
			{ new TypeName("System.Collections", "ICollection"), DmdWellKnownType.System_Collections_ICollection },
			{ new TypeName("System.Collections.Generic", "EqualityComparer`1"), DmdWellKnownType.System_Collections_Generic_EqualityComparer_T },
			{ new TypeName("System.Collections.Generic", "List`1"), DmdWellKnownType.System_Collections_Generic_List_T },
			{ new TypeName("System.Collections.Generic", "IDictionary`2"), DmdWellKnownType.System_Collections_Generic_IDictionary_KV },
			{ new TypeName("System.Collections.Generic", "IReadOnlyDictionary`2"), DmdWellKnownType.System_Collections_Generic_IReadOnlyDictionary_KV },
			{ new TypeName("System.Collections.ObjectModel", "Collection`1"), DmdWellKnownType.System_Collections_ObjectModel_Collection_T },
			{ new TypeName("System.Collections.ObjectModel", "ReadOnlyCollection`1"), DmdWellKnownType.System_Collections_ObjectModel_ReadOnlyCollection_T },
			{ new TypeName("System.Collections.Specialized", "INotifyCollectionChanged"), DmdWellKnownType.System_Collections_Specialized_INotifyCollectionChanged },
			{ new TypeName("System.ComponentModel", "INotifyPropertyChanged"), DmdWellKnownType.System_ComponentModel_INotifyPropertyChanged },
			{ new TypeName("System.ComponentModel", "EditorBrowsableAttribute"), DmdWellKnownType.System_ComponentModel_EditorBrowsableAttribute },
			{ new TypeName("System.ComponentModel", "EditorBrowsableState"), DmdWellKnownType.System_ComponentModel_EditorBrowsableState },
			{ new TypeName("System.Linq", "Enumerable"), DmdWellKnownType.System_Linq_Enumerable },
			{ new TypeName("System.Linq.Expressions", "Expression"), DmdWellKnownType.System_Linq_Expressions_Expression },
			{ new TypeName("System.Linq.Expressions", "Expression`1"), DmdWellKnownType.System_Linq_Expressions_Expression_T },
			{ new TypeName("System.Linq.Expressions", "ParameterExpression"), DmdWellKnownType.System_Linq_Expressions_ParameterExpression },
			{ new TypeName("System.Linq.Expressions", "ElementInit"), DmdWellKnownType.System_Linq_Expressions_ElementInit },
			{ new TypeName("System.Linq.Expressions", "MemberBinding"), DmdWellKnownType.System_Linq_Expressions_MemberBinding },
			{ new TypeName("System.Linq.Expressions", "ExpressionType"), DmdWellKnownType.System_Linq_Expressions_ExpressionType },
			{ new TypeName("System.Linq", "IQueryable"), DmdWellKnownType.System_Linq_IQueryable },
			{ new TypeName("System.Linq", "IQueryable`1"), DmdWellKnownType.System_Linq_IQueryable_T },
			{ new TypeName("System.Xml.Linq", "Extensions"), DmdWellKnownType.System_Xml_Linq_Extensions },
			{ new TypeName("System.Xml.Linq", "XAttribute"), DmdWellKnownType.System_Xml_Linq_XAttribute },
			{ new TypeName("System.Xml.Linq", "XCData"), DmdWellKnownType.System_Xml_Linq_XCData },
			{ new TypeName("System.Xml.Linq", "XComment"), DmdWellKnownType.System_Xml_Linq_XComment },
			{ new TypeName("System.Xml.Linq", "XContainer"), DmdWellKnownType.System_Xml_Linq_XContainer },
			{ new TypeName("System.Xml.Linq", "XDeclaration"), DmdWellKnownType.System_Xml_Linq_XDeclaration },
			{ new TypeName("System.Xml.Linq", "XDocument"), DmdWellKnownType.System_Xml_Linq_XDocument },
			{ new TypeName("System.Xml.Linq", "XElement"), DmdWellKnownType.System_Xml_Linq_XElement },
			{ new TypeName("System.Xml.Linq", "XName"), DmdWellKnownType.System_Xml_Linq_XName },
			{ new TypeName("System.Xml.Linq", "XNamespace"), DmdWellKnownType.System_Xml_Linq_XNamespace },
			{ new TypeName("System.Xml.Linq", "XObject"), DmdWellKnownType.System_Xml_Linq_XObject },
			{ new TypeName("System.Xml.Linq", "XProcessingInstruction"), DmdWellKnownType.System_Xml_Linq_XProcessingInstruction },
			{ new TypeName("System.Security", "UnverifiableCodeAttribute"), DmdWellKnownType.System_Security_UnverifiableCodeAttribute },
			{ new TypeName("System.Security.Permissions", "SecurityAction"), DmdWellKnownType.System_Security_Permissions_SecurityAction },
			{ new TypeName("System.Security.Permissions", "SecurityAttribute"), DmdWellKnownType.System_Security_Permissions_SecurityAttribute },
			{ new TypeName("System.Security.Permissions", "SecurityPermissionAttribute"), DmdWellKnownType.System_Security_Permissions_SecurityPermissionAttribute },
			{ new TypeName("System", "NotSupportedException"), DmdWellKnownType.System_NotSupportedException },
			{ new TypeName("System.Runtime.CompilerServices", "ICriticalNotifyCompletion"), DmdWellKnownType.System_Runtime_CompilerServices_ICriticalNotifyCompletion },
			{ new TypeName("System.Runtime.CompilerServices", "IAsyncStateMachine"), DmdWellKnownType.System_Runtime_CompilerServices_IAsyncStateMachine },
			{ new TypeName("System.Runtime.CompilerServices", "AsyncVoidMethodBuilder"), DmdWellKnownType.System_Runtime_CompilerServices_AsyncVoidMethodBuilder },
			{ new TypeName("System.Runtime.CompilerServices", "AsyncTaskMethodBuilder"), DmdWellKnownType.System_Runtime_CompilerServices_AsyncTaskMethodBuilder },
			{ new TypeName("System.Runtime.CompilerServices", "AsyncTaskMethodBuilder`1"), DmdWellKnownType.System_Runtime_CompilerServices_AsyncTaskMethodBuilder_T },
			{ new TypeName("System.Runtime.CompilerServices", "AsyncStateMachineAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_AsyncStateMachineAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "IteratorStateMachineAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_IteratorStateMachineAttribute },
			{ new TypeName("System.Windows.Forms", "Form"), DmdWellKnownType.System_Windows_Forms_Form },
			{ new TypeName("System.Windows.Forms", "Application"), DmdWellKnownType.System_Windows_Forms_Application },
			{ new TypeName("System", "Environment"), DmdWellKnownType.System_Environment },
			{ new TypeName("System.Runtime", "GCLatencyMode"), DmdWellKnownType.System_Runtime_GCLatencyMode },
			{ new TypeName("System", "IFormatProvider"), DmdWellKnownType.System_IFormatProvider },
			{ new TypeName("System", "ValueTuple`1"), DmdWellKnownType.System_ValueTuple_T1 },
			{ new TypeName("System", "ValueTuple`2"), DmdWellKnownType.System_ValueTuple_T2 },
			{ new TypeName("System", "ValueTuple`3"), DmdWellKnownType.System_ValueTuple_T3 },
			{ new TypeName("System", "ValueTuple`4"), DmdWellKnownType.System_ValueTuple_T4 },
			{ new TypeName("System", "ValueTuple`5"), DmdWellKnownType.System_ValueTuple_T5 },
			{ new TypeName("System", "ValueTuple`6"), DmdWellKnownType.System_ValueTuple_T6 },
			{ new TypeName("System", "ValueTuple`7"), DmdWellKnownType.System_ValueTuple_T7 },
			{ new TypeName("System", "ValueTuple`8"), DmdWellKnownType.System_ValueTuple_TRest },
			{ new TypeName("System.Runtime.CompilerServices", "TupleElementNamesAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_TupleElementNamesAttribute },
			{ new TypeName("System.Runtime.CompilerServices", "ReferenceAssemblyAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_ReferenceAssemblyAttribute },
			{ new TypeName("System", "ContextBoundObject"), DmdWellKnownType.System_ContextBoundObject },
			{ new TypeName("System.Runtime.CompilerServices", "TypeForwardedToAttribute"), DmdWellKnownType.System_Runtime_CompilerServices_TypeForwardedToAttribute },
			{ new TypeName("System.Runtime.InteropServices", "ComImportAttribute"), DmdWellKnownType.System_Runtime_InteropServices_ComImportAttribute },
			{ new TypeName("System.Runtime.InteropServices", "DllImportAttribute"), DmdWellKnownType.System_Runtime_InteropServices_DllImportAttribute },
			{ new TypeName("System.Runtime.InteropServices", "FieldOffsetAttribute"), DmdWellKnownType.System_Runtime_InteropServices_FieldOffsetAttribute },
			{ new TypeName("System.Runtime.InteropServices", "InAttribute"), DmdWellKnownType.System_Runtime_InteropServices_InAttribute },
			{ new TypeName("System.Runtime.InteropServices", "MarshalAsAttribute"), DmdWellKnownType.System_Runtime_InteropServices_MarshalAsAttribute },
			{ new TypeName("System.Runtime.InteropServices", "OptionalAttribute"), DmdWellKnownType.System_Runtime_InteropServices_OptionalAttribute },
			{ new TypeName("System.Runtime.InteropServices", "OutAttribute"), DmdWellKnownType.System_Runtime_InteropServices_OutAttribute },
			{ new TypeName("System.Runtime.InteropServices", "PreserveSigAttribute"), DmdWellKnownType.System_Runtime_InteropServices_PreserveSigAttribute },
			{ new TypeName("System", "SerializableAttribute"), DmdWellKnownType.System_SerializableAttribute },
			{ new TypeName("System.Runtime.InteropServices", "CharSet"), DmdWellKnownType.System_Runtime_InteropServices_CharSet },
			{ new TypeName("System.Reflection", "Assembly"), DmdWellKnownType.System_Reflection_Assembly },
			{ new TypeName("System", "RuntimeMethodHandleInternal"), DmdWellKnownType.System_RuntimeMethodHandleInternal },
			{ new TypeName("System", "ByReference`1"), DmdWellKnownType.System_ByReference_T },
			{ new TypeName("System.Runtime.InteropServices", "UnmanagedType"), DmdWellKnownType.System_Runtime_InteropServices_UnmanagedType },
			{ new TypeName("System.Runtime.InteropServices", "VarEnum"), DmdWellKnownType.System_Runtime_InteropServices_VarEnum },
			{ new TypeName("System", "__ComObject"), DmdWellKnownType.System___ComObject },
			{ new TypeName("System.Runtime.InteropServices.WindowsRuntime", "RuntimeClass"), DmdWellKnownType.System_Runtime_InteropServices_WindowsRuntime_RuntimeClass },
			{ new TypeName("System", "DBNull"), DmdWellKnownType.System_DBNull },
			{ new TypeName("System.Security.Permissions", "PermissionSetAttribute"), DmdWellKnownType.System_Security_Permissions_PermissionSetAttribute },
			{ new TypeName("System.Diagnostics", "DebuggerTypeProxyAttribute"), DmdWellKnownType.System_Diagnostics_DebuggerTypeProxyAttribute },
			{ new TypeName("System.Collections.Generic", "KeyValuePair`2"), DmdWellKnownType.System_Collections_Generic_KeyValuePair_T2 },
		};
		static readonly Dictionary<string, DmdWellKnownType> toNestedWellKnownType = new Dictionary<string, DmdWellKnownType>(WELL_KNOWN_NESTED_TYPES_COUNT, StringComparer.Ordinal) {
			{ "ForLoopControl", DmdWellKnownType.Microsoft_VisualBasic_CompilerServices_ObjectFlowControl_ForLoopControl },
			{ "DebuggingModes", DmdWellKnownType.System_Diagnostics_DebuggableAttribute__DebuggingModes },
			{ "CrossThreadDependencyNotification", DmdWellKnownType.System_Diagnostics_Debugger_CrossThreadDependencyNotification },
		};

		static WellKnownMemberResolver() {
			if (toNonNestedWellKnownType.Count != WELL_KNOWN_NONNESTED_TYPES_COUNT)
				throw new InvalidOperationException();
			if (toNestedWellKnownType.Count != WELL_KNOWN_NESTED_TYPES_COUNT)
				throw new InvalidOperationException();
			if (typeof(DmdWellKnownType).GetFields().Where(a => a.IsLiteral).Count() != WELL_KNOWN_TYPES_COUNT)
				throw new InvalidOperationException();
		}
	}
}
