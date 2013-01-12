using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMediaKit.DirectShow.Interop;
using System.Runtime.InteropServices;

namespace VrPlayer.Helpers.Media
{
    public class GraphUtil
    {
        public GraphUtil()
        {

        }

        public static StringBuilder EnumFilters(IGraphBuilder graphBuilder)
        {
            StringBuilder sb = new StringBuilder();
            IEnumFilters filtersEnum;
            int hr = graphBuilder.EnumFilters(out filtersEnum);
            DsError.ThrowExceptionForHR(hr);

            int numFiltersToGet = 1;
            IBaseFilter[] filters = new IBaseFilter[numFiltersToGet];
            IntPtr fetched = IntPtr.Zero;
            while (true)
            {
                hr = filtersEnum.Next(numFiltersToGet, filters, fetched);
                DsError.ThrowExceptionForHR(hr);
                if ((int)fetched < 1)
                {
                    break;
                }
                IBaseFilter filter = filters[0];
                FilterInfo filterInfo;
                hr = filter.QueryFilterInfo(out filterInfo);
                DsError.ThrowExceptionForHR(hr);
                //DumpLine("FILTER " + filterInfo.achName);
                sb.Append("FILTER " + filterInfo.achName + "\r\n");

                EnumPins(filter, sb);

                // The FILTER_INFO structure holds a pointer to the Filter Graph Manager, 
                //with a reference count that must be released.
                if (filterInfo.pGraph != null)
                {
                    Marshal.ReleaseComObject(filterInfo.pGraph);
                }
                Marshal.ReleaseComObject(filter);

            }
            Marshal.ReleaseComObject(filtersEnum);
            return sb;
        }

        public static void EnumPins(IBaseFilter filter, StringBuilder sb)
        {
            int numPinsToGet = 1;
            IntPtr fetched = IntPtr.Zero;
            IEnumPins pinsEnum;
            int hr = filter.EnumPins(out pinsEnum);
            DsError.ThrowExceptionForHR(hr);
            IPin[] pins = new IPin[numPinsToGet];
            while (true)
            {
                hr = pinsEnum.Next(numPinsToGet, pins, fetched);
                DsError.ThrowExceptionForHR(hr);
                if ((int)fetched < 1)
                {
                    break;
                }
                IPin pin = pins[0];
                PinInfo pinInfo;
                hr = pin.QueryPinInfo(out pinInfo);
                DsError.ThrowExceptionForHR(hr);

                //DumpLine("  PIN " + pinInfo.dir.ToString() + " " + pinInfo.name);
                sb.Append("  PIN " + pinInfo.dir.ToString() + " " + pinInfo.name);
                string filterName = PinConnectedToFilter(pin);
                if (pinInfo.dir == PinDirection.Output)
                {
                    sb.Append(" >>> " + filterName);
                }
                if (pinInfo.dir == PinDirection.Input)
                {
                    sb.Append(" <<< " + filterName);
                }
                sb.Append("\r\n");

                EnumMediaTypes(pin, sb);

                Marshal.ReleaseComObject(pin);
            }

            Marshal.ReleaseComObject(pinsEnum);
        }

        public static string PinConnectedToFilter(IPin pin)
        {
            string filterName = String.Empty;

            IPin destPin;
            int hr = pin.ConnectedTo(out destPin);
            //DsError.ThrowExceptionForHR(hr);

            if (hr == 0) //its connected
            {
                PinInfo destPinInfo;
                hr = destPin.QueryPinInfo(out destPinInfo);
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter destFilter = destPinInfo.filter;
                FilterInfo destFilterInfo;
                hr = destFilter.QueryFilterInfo(out destFilterInfo);
                DsError.ThrowExceptionForHR(hr);

                filterName = destFilterInfo.achName;
            }

            return filterName;
        }

        public static void EnumMediaTypes(IPin pin, StringBuilder sb)
        {
            int numMediaToGet = 1;
            IntPtr fetched = IntPtr.Zero;
            IEnumMediaTypes mediaTypesEnum;
            int hr = pin.EnumMediaTypes(out mediaTypesEnum);
            DsError.ThrowExceptionForHR(hr);
            AMMediaType[] mediaTypes = new AMMediaType[numMediaToGet];
            while (true)
            {
                hr = mediaTypesEnum.Next(numMediaToGet, mediaTypes, fetched);
                DsError.ThrowExceptionForHR(hr);
                if ((int)fetched < 1)
                {
                    break;
                }
                AMMediaType mediaType = mediaTypes[0];

                //DumpLine("    MEDIATYPE ...");

                //Marshal.ReleaseComObject(mediaType);
            }
            Marshal.ReleaseComObject(mediaTypesEnum);
        }

        public static void SaveGraphFile(IGraphBuilder graphBuilder, string fileName)
        {
            int hr = 0;
            IStorage graphStorage = null;
            System.Runtime.InteropServices.ComTypes.IStream stream = null;
            try
            {
                // Get the graph's persist stream interface
                IPersistStream ps = (IPersistStream)graphBuilder;

                STGM grfMode = STGM.Create | STGM.Transacted | STGM.ReadWrite | STGM.ShareExclusive;
                hr = NativeMethods.StgCreateDocfile(fileName, grfMode, 0, out graphStorage);
                Marshal.ThrowExceptionForHR(hr);

                grfMode = STGM.Write | STGM.Create | STGM.ShareExclusive;
                hr = graphStorage.CreateStream("ActiveMovieGraph", grfMode, 0, 0, out stream);
                DsError.ThrowExceptionForHR(hr);

                ps.Save(stream, true);
                graphStorage.Commit(0);
            }
            finally
            {
                if (stream != null)
                    Marshal.ReleaseComObject(stream);
                if (graphStorage != null)
                    Marshal.ReleaseComObject(graphStorage);
            }
        }

        //http://sourceforge.net/forum/message.php?msg_id=3527574
        public static void LoadGraphFile(IGraphBuilder graphBuilder, string fileName)
        {
            int hr = 0;
            IStorage storage = null;
            System.Runtime.InteropServices.ComTypes.IStream stream = null;

            try
            {
                if (NativeMethods.StgIsStorageFile(fileName) != 0)
                    return;

                hr = NativeMethods.StgOpenStorage(
                    fileName,
                    null,
                    STGM.Transacted | STGM.Read | STGM.ShareDenyWrite,
                    IntPtr.Zero,
                    0,
                    out storage
                    );

                Marshal.ThrowExceptionForHR(hr);

                hr = storage.OpenStream(
                    @"ActiveMovieGraph",
                    IntPtr.Zero,
                    STGM.Read | STGM.ShareExclusive,
                    0,
                    out stream
                    );

                if (hr >= 0)
                {
                    hr = (graphBuilder as IPersistStream).Load(stream);
                }
            }
            finally
            {
                if (stream != null)
                    Marshal.ReleaseComObject(stream);
                if (storage != null)
                    Marshal.ReleaseComObject(storage);
            }
        }

        internal sealed class NativeMethods
        {
            private NativeMethods() { }

            [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int StgCreateDocfile(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] STGM grfMode,
                [In] int reserved,
                [Out] out IStorage ppstgOpen
                );

            [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int StgIsStorageFile([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

            [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int StgOpenStorage(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] IStorage pstgPriority,
                [In] STGM grfMode,
                [In] IntPtr snbExclude,
                [In] int reserved,
                [Out] out IStorage ppstgOpen
                );

        }

        [Flags]
        internal enum STGM
        {
            Read = 0x00000000,
            Write = 0x00000001,
            ReadWrite = 0x00000002,
            ShareDenyNone = 0x00000040,
            ShareDenyRead = 0x00000030,
            ShareDenyWrite = 0x00000020,
            ShareExclusive = 0x00000010,
            Priority = 0x00040000,
            Create = 0x00001000,
            Convert = 0x00020000,
            FailIfThere = 0x00000000,
            Direct = 0x00000000,
            Transacted = 0x00010000,
            NoScratch = 0x00100000,
            NoSnapShot = 0x00200000,
            Simple = 0x08000000,
            DirectSWMR = 0x00400000,
            DeleteOnRelease = 0x04000000,
        }

        [Flags]
        internal enum STGC
        {
            Default = 0,
            Overwrite = 1,
            OnlyIfCurrent = 2,
            DangerouslyCommitMerelyToDiskCache = 4,
            Consolidate = 8
        }

        [Guid("0000000b-0000-0000-C000-000000000046"),
            InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IStorage
        {
            [PreserveSig]
            int CreateStream(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] STGM grfMode,
                [In] int reserved1,
                [In] int reserved2,
               [Out] out System.Runtime.InteropServices.ComTypes.IStream ppstm
                );

            [PreserveSig]
            int OpenStream(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] IntPtr reserved1,
                [In] STGM grfMode,
                [In] int reserved2,
               [Out] out System.Runtime.InteropServices.ComTypes.IStream ppstm
                );

            [PreserveSig]
            int CreateStorage(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] STGM grfMode,
                [In] int reserved1,
                [In] int reserved2,
                [Out] out IStorage ppstg
                );

            [PreserveSig]
            int OpenStorage(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] IStorage pstgPriority,
                [In] STGM grfMode,
                [In] int snbExclude,
                [In] int reserved,
                [Out] out IStorage ppstg
                );

            [PreserveSig]
            int CopyTo(
                [In] int ciidExclude,
                [In] Guid[] rgiidExclude,
                [In] string[] snbExclude,
                [In] IStorage pstgDest
                );

            [PreserveSig]
            int MoveElementTo(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] IStorage pstgDest,
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName,
                [In] STGM grfFlags
                );

            [PreserveSig]
            int Commit([In] STGC grfCommitFlags);

            [PreserveSig]
            int Revert();

            [PreserveSig]
            int EnumElements(
                [In] int reserved1,
                [In] IntPtr reserved2,
                [In] int reserved3,
                [Out, MarshalAs(UnmanagedType.Interface)] out object ppenum
                );

            [PreserveSig]
            int DestroyElement([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

            [PreserveSig]
            int RenameElement(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName,
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName
                );

            [PreserveSig]
            int SetElementTimes(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
                [In] FILETIME pctime,
                [In] FILETIME patime,
                [In] FILETIME pmtime
                );

            [PreserveSig]
            int SetClass([In, MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

            [PreserveSig]
            int SetStateBits(
                [In] int grfStateBits,
                [In] int grfMask
                );

            [PreserveSig]
            int Stat(
                [Out] out STATSTG pStatStg,
                [In] int grfStatFlag
                );
        }
    }
}
