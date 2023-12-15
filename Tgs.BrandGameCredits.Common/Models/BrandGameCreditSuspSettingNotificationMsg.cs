using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.BrandGameCredits.Common.Models;

public class BrandGameCreditSuspSettingNotificationMsg
{
    public string EmailSubject { get; init; } = default!;
    public string EmailContent { get; init; } = default!;
}
