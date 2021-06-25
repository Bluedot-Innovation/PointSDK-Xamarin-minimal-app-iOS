#!/bin/bash
APP_IPA="${APPCENTER_OUTPUT_DIRECTORY}/BDPointiOSXamarinDemo.ipa"
IPA="${APPCENTER_OUTPUT_DIRECTORY}/BDPointiOSXamarinDemo.ipa"
TEMP_IPA_BUILT="/tmp/ipabuild"

echo "APP_IPA = ${APP_IPA}"
echo "IPA = ${IPA}" 

DEVELOPER_DIR=`xcode-select --print-path`
if [ ! -d "${DEVELOPER_DIR}" ]; then
	echo "No developer directory found!"
	exit 1
fi

echo "+ Packaging SwiftSupport into ${IPA}"

if [ -f "${IPA}" ] && [ "${IPA}" != "${APP_IPA}" ]; then
    /bin/rm "${IPA}"
fi    
if [ -d "${TEMP_IPA_BUILT}" ]; then
    rm -rf "${TEMP_IPA_BUILT}"
fi

echo "+ Unzip ipa content"
unzip -q "$APP_IPA" -d "$TEMP_IPA_BUILT"

echo "+ Adding SWIFT support"
mkdir -p "${TEMP_IPA_BUILT}/SwiftSupport"
cd "${TEMP_IPA_BUILT}/Payload/BDPointiOSXamarinDemo.app/Frameworks/"    
for SWIFT_LIB in libswift*.dylib; do
    echo "Copying ${SWIFT_LIB}"
    cp $SWIFT_LIB "${TEMP_IPA_BUILT}/SwiftSupport"
done

echo "+ zip --symlinks --verbose --recurse-paths ${IPA} ."
cd "${TEMP_IPA_BUILT}"
zip --symlinks --verbose --recurse-paths "${IPA}" .