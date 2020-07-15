#include "log.hpp"

namespace Entitas
{
	SyncLog logDelegate;

	
	#include "type.hpp"
	
	std::ofstream Log::m_error_log_file;
	std::ofstream Log::m_info_log_file;
	std::ofstream Log::m_warn_log_file;
	
	
	void InitLogger(const std::string& info_log_filename, const std::string& warn_log_filename, const std::string& error_log_filename) {
		Log::m_info_log_file.open(info_log_filename.c_str());
		Log::m_warn_log_file.open(warn_log_filename.c_str());
		Log::m_error_log_file.open(error_log_filename.c_str());
	}
	
	
	Log::~Log(void)
	{
		GetStream(m_log_level) << std::endl << std::flush;
		if (FATAL == m_log_level)
		{
			m_info_log_file.close();
			m_info_log_file.close();
			m_info_log_file.close();
			abort();
		}
	}
	
	
	std::ostream& Log::GetStream(LogLevel level) {
		return (INFO == level) ?
			(m_info_log_file.is_open() ? m_info_log_file : std::cout) :
			(WARN == level ?
			(m_warn_log_file.is_open() ? m_warn_log_file : std::cerr) :
				(m_error_log_file.is_open() ? m_error_log_file : std::cerr));
	}
	
	
	std::ostream& Log::Start(LogLevel level, std::string text, const std::string &file, const int line, const std::string &func)
	{
		time_t tm;
		time(&tm);
		char tmp[64];
		strftime(tmp, sizeof(tmp), "%Y-%m-%d %H:%M:%S", localtime(&tm));
		size_t t = file.find_last_of('\\');
		std::string vl = text + " \n@" + file.substr(t + 1) + ":" + tostring(line);
		if (logDelegate)
		{
			switch (level)
			{
			case INFO:
				logDelegate(vl.c_str(), CLog);
				break;
			case WARN:
				logDelegate(vl.c_str(), CWarn);
				break;
			case ERR:
            case FATAL:
				logDelegate(vl.c_str(), CError);
				break;
			}
		}
		std::ostream& ostr = GetStream(level);
		ostr << tmp << " " << func << " at:" << file.substr(t + 1) << ":" << line << std::endl;
		return  ostr;
	}
	
	
	std::ostream& Log::OStart(const std::string &file, const int line, const std::string &func)
	{
		time_t tm;
		time(&tm);
		char time_string[128];
		strftime(time_string, sizeof(time_string), "%Y-%m-%d %H:%M:%S ", localtime(&tm));
		size_t t = file.find_last_of('\\');
		return GetStream(INFO) << time_string << func << " at:" << file.substr(t + 1) << ":" << line << " \n" << std::flush;
	}
}
