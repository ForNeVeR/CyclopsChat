#--------------------------------------------------------------------------
# Copyrights
#
# Portions created by or assigned to Cursive Systems, Inc. are
# Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
# information for Cursive Systems, Inc. is available at
# http://www.cursive.net/.
#
# License
#
# Jabber-Net can be used under either JOSL or the GPL.
# See LICENSE.txt for details.
# --------------------------------------------------------------------------

# The purpose of this Makefile is to facilitate mono builds.

DEBUG = -debug
BASEDIR = $(CURDIR)

SOURCES = \
$(BASEDIR)/AssemblyInfo.cs \
$(BASEDIR)/bedrock/Delegates.cs \
$(BASEDIR)/bedrock/collections/ByteStack.cs \
$(BASEDIR)/bedrock/collections/GraphNode.cs \
$(BASEDIR)/bedrock/collections/ISet.cs \
$(BASEDIR)/bedrock/collections/IndexedTrie.cs \
$(BASEDIR)/bedrock/collections/LinkedList.cs \
$(BASEDIR)/bedrock/collections/Set.cs \
$(BASEDIR)/bedrock/collections/SkipList.cs \
$(BASEDIR)/bedrock/collections/StringSet.cs \
$(BASEDIR)/bedrock/collections/Tree.cs \
$(BASEDIR)/bedrock/collections/Trie.cs \
$(BASEDIR)/bedrock/collections/TrieNode.cs \
$(BASEDIR)/bedrock/io/BufferAggregate.cs \
$(BASEDIR)/bedrock/io/ReadEventStream.cs \
$(BASEDIR)/bedrock/io/ZlibStream.cs \
$(BASEDIR)/bedrock/net/Address.cs \
$(BASEDIR)/bedrock/net/AsyncSocket.cs \
$(BASEDIR)/bedrock/net/BaseSocket.cs \
$(BASEDIR)/bedrock/net/Exceptions.cs \
$(BASEDIR)/bedrock/net/HttpSocket.cs \
$(BASEDIR)/bedrock/net/IHttpSocket.cs \
$(BASEDIR)/bedrock/net/XEP25Socket.cs \
$(BASEDIR)/bedrock/net/XEP124Socket.cs \
$(BASEDIR)/bedrock/net/ProxySocket.cs \
$(BASEDIR)/bedrock/net/ShttpProxy.cs \
$(BASEDIR)/bedrock/net/SocketEventListener.cs \
$(BASEDIR)/bedrock/net/SocketWatcher.cs \
$(BASEDIR)/bedrock/net/Socks4Proxy.cs \
$(BASEDIR)/bedrock/net/Socks5Proxy.cs \
$(BASEDIR)/bedrock/util/ConfigFile.cs \
$(BASEDIR)/bedrock/util/GetOptBase.cs \
$(BASEDIR)/bedrock/util/Version.cs \
$(BASEDIR)/jabber/JID.cs \
$(BASEDIR)/jabber/client/BookmarkManager.cs \
$(BASEDIR)/jabber/client/JabberClient.cs \
$(BASEDIR)/jabber/client/PresenceManager.cs \
$(BASEDIR)/jabber/client/RosterManager.cs \
$(BASEDIR)/jabber/connection/BindingStanzaStream.cs \
$(BASEDIR)/jabber/connection/CapsManager.cs \
$(BASEDIR)/jabber/connection/ConferenceManager.cs \
$(BASEDIR)/jabber/connection/DiscoManager.cs \
$(BASEDIR)/jabber/connection/FileMap.cs \
$(BASEDIR)/jabber/connection/HttpStanzaStream.cs \
$(BASEDIR)/jabber/connection/IQTracker.cs \
$(BASEDIR)/jabber/connection/PollingStanzaStream.cs \
$(BASEDIR)/jabber/connection/PubSubManager.cs \
$(BASEDIR)/jabber/connection/SocketStanzaStream.cs \
$(BASEDIR)/jabber/connection/StanzaStream.cs \
$(BASEDIR)/jabber/connection/States.cs \
$(BASEDIR)/jabber/connection/StreamComponent.cs \
$(BASEDIR)/jabber/connection/XmppStream.cs \
$(BASEDIR)/jabber/connection/sasl/ExternalProcessor.cs \
$(BASEDIR)/jabber/connection/sasl/KerbProcessor.cs \
$(BASEDIR)/jabber/connection/sasl/MD5Processor.cs \
$(BASEDIR)/jabber/connection/sasl/PlainProcessor.cs \
$(BASEDIR)/jabber/connection/sasl/SASLProcessor.cs \
$(BASEDIR)/jabber/protocol/AsynchElementStream.cs \
$(BASEDIR)/jabber/protocol/Element.cs \
$(BASEDIR)/jabber/protocol/ElementFactory.cs \
$(BASEDIR)/jabber/protocol/ElementList.cs \
$(BASEDIR)/jabber/protocol/ElementStream.cs \
$(BASEDIR)/jabber/protocol/EnumParser.cs \
$(BASEDIR)/jabber/protocol/NS.cs \
$(BASEDIR)/jabber/protocol/Packet.cs \
$(BASEDIR)/jabber/protocol/URI.cs \
$(BASEDIR)/jabber/protocol/accept/Factory.cs \
$(BASEDIR)/jabber/protocol/accept/Handshake.cs \
$(BASEDIR)/jabber/protocol/accept/Log.cs \
$(BASEDIR)/jabber/protocol/accept/Route.cs \
$(BASEDIR)/jabber/protocol/accept/Xdb.cs \
$(BASEDIR)/jabber/protocol/client/Error.cs \
$(BASEDIR)/jabber/protocol/client/Factory.cs \
$(BASEDIR)/jabber/protocol/client/IQ.cs \
$(BASEDIR)/jabber/protocol/client/Message.cs \
$(BASEDIR)/jabber/protocol/client/Presence.cs \
$(BASEDIR)/jabber/protocol/client/ProtocolException.cs \
$(BASEDIR)/jabber/protocol/iq/Agents.cs \
$(BASEDIR)/jabber/protocol/iq/Auth.cs \
$(BASEDIR)/jabber/protocol/iq/Bookmarks.cs \
$(BASEDIR)/jabber/protocol/iq/Browse.cs \
$(BASEDIR)/jabber/protocol/iq/Disco.cs \
$(BASEDIR)/jabber/protocol/iq/Factory.cs \
$(BASEDIR)/jabber/protocol/iq/GeoLoc.cs \
$(BASEDIR)/jabber/protocol/iq/Last.cs \
$(BASEDIR)/jabber/protocol/iq/MUC.cs \
$(BASEDIR)/jabber/protocol/iq/OOB.cs \
$(BASEDIR)/jabber/protocol/iq/Private.cs \
$(BASEDIR)/jabber/protocol/iq/PubSub.cs \
$(BASEDIR)/jabber/protocol/iq/PubSubErrors.cs \
$(BASEDIR)/jabber/protocol/iq/PubSubEvent.cs \
$(BASEDIR)/jabber/protocol/iq/PubSubOwner.cs \
$(BASEDIR)/jabber/protocol/iq/Register.cs \
$(BASEDIR)/jabber/protocol/iq/Roster.cs \
$(BASEDIR)/jabber/protocol/iq/Time.cs \
$(BASEDIR)/jabber/protocol/iq/VCard.cs \
$(BASEDIR)/jabber/protocol/iq/Version.cs \
$(BASEDIR)/jabber/protocol/stream/Bind.cs \
$(BASEDIR)/jabber/protocol/stream/Compression.cs \
$(BASEDIR)/jabber/protocol/stream/Error.cs \
$(BASEDIR)/jabber/protocol/stream/Factory.cs \
$(BASEDIR)/jabber/protocol/stream/Features.cs \
$(BASEDIR)/jabber/protocol/stream/HttpBind.cs \
$(BASEDIR)/jabber/protocol/stream/SASL.cs \
$(BASEDIR)/jabber/protocol/stream/Session.cs \
$(BASEDIR)/jabber/protocol/stream/StartTLS.cs \
$(BASEDIR)/jabber/protocol/stream/Stream.cs \
$(BASEDIR)/jabber/protocol/x/Caps.cs \
$(BASEDIR)/jabber/protocol/x/Data.cs \
$(BASEDIR)/jabber/protocol/x/Delay.cs \
$(BASEDIR)/jabber/protocol/x/Event.cs \
$(BASEDIR)/jabber/protocol/x/Factory.cs \
$(BASEDIR)/jabber/server/JabberService.cs \
$(BASEDIR)/jabber/server/XdbTracker.cs \
$(BASEDIR)/xpnet/ContentToken.cs \
$(BASEDIR)/xpnet/Encoding.cs \
$(BASEDIR)/xpnet/Exceptions.cs \
$(BASEDIR)/xpnet/Position.cs \
$(BASEDIR)/xpnet/Token.cs \
$(BASEDIR)/xpnet/UTF8Encoding.cs \
$(BASEDIR)/stringprep/Nameprep.cs \
$(BASEDIR)/stringprep/Plain.cs \
$(BASEDIR)/stringprep/Profile.cs \
$(BASEDIR)/stringprep/steps/BidiStep.cs \
$(BASEDIR)/stringprep/steps/MapStep.cs \
$(BASEDIR)/stringprep/steps/NFKCStep.cs \
$(BASEDIR)/stringprep/steps/ProfileStep.cs \
$(BASEDIR)/stringprep/steps/ProhibitStep.cs \
$(BASEDIR)/stringprep/steps/RFC3454.cs \
$(BASEDIR)/stringprep/unicode/Combining.cs \
$(BASEDIR)/stringprep/unicode/CombiningData.cs \
$(BASEDIR)/stringprep/unicode/Compose.cs \
$(BASEDIR)/stringprep/unicode/ComposeData.cs \
$(BASEDIR)/stringprep/unicode/Decompose.cs \
$(BASEDIR)/stringprep/unicode/DecomposeData.cs \
$(BASEDIR)/stringprep/XmppNode.cs \
$(BASEDIR)/stringprep/XmppResource.cs


RESOURCES = \
-resource:$(BASEDIR)/jabber/client/JabberClient.resx \
-resource:$(BASEDIR)/jabber/client/RosterManager.resx \
-resource:$(BASEDIR)/jabber/connection/XmppStream.resx \
-resource:$(BASEDIR)/jabber/server/JabberService.resx \
-resource:$(BASEDIR)/jabber/client/JabberClient.bmp \
-resource:$(BASEDIR)/jabber/client/PresenceManager.bmp \
-resource:$(BASEDIR)/jabber/client/RosterManager.bmp \
-resource:$(BASEDIR)/jabber/server/JabberService.bmp

SYSTEM_REFERENCES = -r:zlib.net.dll \
					-r:System.dll \
					-r:System.Xml.dll \
					-r:System.Drawing.dll \
					-r:Mono.Security.dll

DEBUGDIR = $(BASEDIR)/bin/debug

MCS=gmcs

MCS_OPTIONS = -lib:$(DEBUGDIR),$(BASEDIR)/lib20 $(DEBUG) -define:DEBUG

ASSEMBLIES =
DLL = $(DEBUGDIR)/jabber-net.dll

SUBDIRS = ConsoleClient test
.PHONY: all clean $(SUBDIRS)

all:  $(DLL) $(SUBDIRS)

dll : $(DLL)

$(DLL): $(SOURCES)
	-mkdir -p bin/debug
	$(MCS) $(MCS_OPTIONS) -target:library \
	-out:"$@" $(RESOURCES) $(SYSTEM_REFERENCES) \
	$^ $(ASSEMBLIES) 

$(SUBDIRS): $(DLL)
	$(MAKE) -C $@

clean:
	$(RM) -r bin
	for dir in $(SUBDIRS); do \
		$(MAKE) -C $$dir $@; \
	done	

