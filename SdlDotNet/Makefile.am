SUBDIRS = src \
	  extras \
	  examples \
	  tests

pkgconfig_in_files = \
	sdldotnet.pc.in

pkgconfigdir=$(prefix)/lib/pkgconfig
pkgconfig_DATA=$(pkgconfig_in_files:.pc.in=.pc)

EXTRA_DIST = \
	$(pkgconfig_in_files) \
	am-config.mk

DISTCLEANFILES = \
	configure \
	Makefile.in  \
	aclocal.m4
