/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using log4net;
using log4net.Config;

namespace MfGames.Utility
{
  public abstract class LoggedObject
  {
    #region Context
    // Contains the logging interface, uninitialized until it is first
    // used.
    private ILog log = null;
    
    /// <summary>
    /// Returns the Type that represents the context for this
    /// class. This defaults to the class itself.
    /// </summary>
    protected virtual Type LoggingContext
    {
      get { return GetType(); }
    }

    // Also try "String.Format("{0}: ", ToString())
    protected virtual string LoggingID
    {
      get { return ""; }
    }

    /// <summary>
    /// Initializes the logging system, connecting the log4net system
    /// to this object using the LoggingContext for the context to
    /// use.
    /// </summary>
    private void InitLogging()
    {
      // Initialize the logging system. This does not check for the
      // log object being null since we assume it has already been tested.
      log = LogManager.GetLogger(LoggingContext);
    }
    #endregion

    #region Logging
    public void Debug(string msg)
    {
      // Ensure logging is prepared
      if (log == null)
	InitLogging();

      // Log the message
      if (log.IsDebugEnabled)
	log.Debug(String.Format("{0}{1}", LoggingID, msg));
    }

    public void Debug(string fmt, params object [] args)
    {
      Debug(String.Format(fmt, args));
    }

    public void Error(string msg)
    {
      // Ensure logging is prepared
      if (log == null)
	InitLogging();

      // Log the message
      if (log.IsErrorEnabled)
	log.Error(String.Format("{0}{1}", LoggingID, msg));
    }

    public void Error(string fmt, params object [] args)
    {
      Error(String.Format(fmt, args));
    }

    public void Info(string msg)
    {
      // Ensure logging is prepared
      if (log == null)
	InitLogging();

      // Log the message
      if (log.IsInfoEnabled)
	log.Info(String.Format("{0}{1}", LoggingID, msg));
    }

    public void Info(string fmt, params object [] args)
    {
      Info(String.Format(fmt, args));
    }
    #endregion
  }
}
