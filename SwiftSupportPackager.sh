#!/bin/bash
APP_IPA="$1"
IPA="$2"
ARCHIVE_PATH="$3"
TEMP_IPA_BUILT="/tmp/ipabuild"

DEVELOPER_DIR=`xcode-select --print-path`
if [ ! -d "${DEVELOPER_DIR}" ]; then
	echo "No developer directory found!"
	exit 1
fi

echo "+ Packaging ${APP} into ${IPA}"

if [ -f "${IPA}" ];
then
    /bin/rm "${IPA}"
fi    
if [ -d "${TEMP_IPA_BUILT}" ];
then
    rm -rf "${TEMP_IPA_BUILT}"
fi

echo "+ Unzip ipa content"
unzip -q "$APP_IPA" -d "$TEMP_IPA_BUILT"

echo "+ Create SwiftSupport folder"
mkdir -p "${TEMP_IPA_BUILT}/SwiftSupport"

echo "+ Copy SwiftSupport libraries"
cp -R "${ARCHIVE_PATH}/SwiftSupport/iphoneos" "${TEMP_IPA_BUILT}/SwiftSupport/iphoneos"

echo "+ zip --symlinks --verbose --recurse-paths ${IPA} ."
cd "${TEMP_IPA_BUILT}"
zip --symlinks --verbose --recurse-paths "${IPA}" .