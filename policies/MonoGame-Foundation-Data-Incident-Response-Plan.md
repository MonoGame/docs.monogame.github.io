---
title: Data Incident Response & Data Protection Operations Plan
layout: landing
_disableToc: true
_disableBreadcrumb: true
---

||
|-|
|**Version 1.0**|
|**Effective date:** 1st August 2026|
|**Classification:** Public|
|**Review cadence:** Annually, and after every activation|

---

# Data Incident Response & Data Protection Operations Plan

This plan gives the MonoGame Foundation, Inc. ("the Foundation") a documented, rehearsable procedure for (a) responding to data security incidents and breaches, including the notification duties under Texas Business & Commerce Code § 521.053 and GDPR Articles 33–34, and (b) handling day-to-day data protection operations: rights requests, privacy queries, retention and destruction, and vendor management. It is the internal counterpart to the public commitments in the Foundation's Privacy Policy.

## Part A — Roles and Contacts

| Role | Holder | Responsibilities |
|---|---|---|
| Incident Lead (Data Protection Officer-equivalent) | Foundation President | Owns this plan; declares incidents; decides notification; signs regulator filings |
| Technical Lead | Foundation Board Chairman | Containment, forensics, log preservation, recovery |
| Communications Lead | Foundation Corporate Secretary | User notices, transparency posts, press/community questions |
| Legal Counsel | Foundation Treasurer | Notification-duty analysis, regulator liaison |
| Board liaison | Foundation Corporate Secretary | Board notification, resource decisions |

Standing mailboxes: **`privacy@monogame.net`** (rights requests and privacy queries), **`privacy@monogame.net`** (vulnerability and incident reports), **`admin@monogame.net`** (copyright notices). Each must route to at least two named people.

Key external contacts to keep current in this section: Discourse/CDCK support and `privacy@discourse.org`; hosting provider abuse/security contact; payment processor security contacts; the Texas Attorney General breach reporting portal (`texasattorneygeneral.gov`); cyber-insurance carrier if/when obtained.

## Part B — Incident Response Procedure

### B.1 Definitions

- **Security incident:** any event that compromises, or credibly threatens, the confidentiality, integrity, or availability of Foundation systems or data (compromised admin account, exploited vulnerability, lost backup, ransomware, processor breach notice).
- **Personal data breach (GDPR):** a breach of security leading to accidental or unlawful destruction, loss, alteration, unauthorized disclosure of, or access to personal data.
- **Breach of system security (Texas § 521.053(a)):** unauthorized acquisition of computerized data that compromises the security, confidentiality, or integrity of *sensitive personal information* — broadly, a name combined with SSN, government ID number, or financial account/card numbers with access codes, or certain health data. Encrypted data counts if the key was also taken.

A single event can be all three. The GDPR definition is the broadest; assess against it first.

### B.2 Severity classification

| Level | Definition | Examples | Activation |
|---|---|---|---|
| SEV-1 | Confirmed unauthorized acquisition of personal data, or full admin compromise | Database exfiltration; stolen admin credentials used; processor reports breach of our data | Full plan; all roles; notification analysis mandatory |
| SEV-2 | Suspected exposure or serious vulnerability under active exploitation | Credential stuffing spike; vulnerability disclosed with evidence of probing | Incident Lead + Technical Lead; 72-hour assessment |
| SEV-3 | Contained or low-risk event | Single account phished and recovered; vulnerability patched before exploitation | Technical Lead; log in incident register |

### B.3 Response phases and the notification clocks

**Phase 1 — Detect & contain (hour 0+).** Confirm the report; preserve logs and evidence *before* remediation where possible; contain (revoke sessions/keys, isolate systems, force password resets if credentials affected); open an incident record (timestamps matter — every legal clock runs from when the Foundation *determines* a breach occurred or *becomes aware* of it).

**Phase 2 — Assess (within 72 hours of awareness).** Establish: what data, whose data, how many people, which jurisdictions (forum location data is unreliable; assume EU/UK members are affected unless shown otherwise), whether Texas "sensitive personal information" is involved, whether data was encrypted and keys safe, and the risk to individuals. Record the determination and its time. Engage counsel for any SEV-1.

**Phase 3 — Notify.** The clocks, fastest first:

| Audience | Trigger | Deadline | Method |
|---|---|---|---|
| EU/UK supervisory authority | Personal data breach with risk to individuals, GDPR applies | **72 hours** from awareness (document the reason for any delay) | Authority's online breach form |
| Affected individuals (GDPR) | High risk to rights and freedoms | **Without undue delay** | Email + transparency post |
| Texas Attorney General | Breach of system security involving **≥ 250 Texas residents** | **As soon as practicable, ≤ 30 days** from determination | **Electronic form on the AG website** (mandatory); include nature of breach, number of affected Texans, notices sent, measures taken and planned, law-enforcement status. Note: the AG publishes reported breaches publicly. |
| Affected individuals (Texas) | Unauthorized acquisition of their sensitive personal information | **Without unreasonable delay, ≤ 60 days** from determination (limited delay for law enforcement / scoping) | Written or email notice; substitute notice (site posting + statewide media) only if cost > $250,000, > 500,000 affected, or insufficient contact information is held (per the Texas AG's ITEPA guidance) |
| Other US states' residents | Per each state's breach statute | Varies — counsel to run a 50-state check for SEV-1 | Per statute |
| Consumer reporting agencies | **> 10,000 persons** notified at once | Without unreasonable delay | Equifax, Experian, TransUnion security contacts |
| Processors/vendors | Their systems implicated | Immediately | Direct contact (conversely: our processor contracts must require them to notify us without undue delay) |
| Community at large | Any SEV-1 affecting forum data | With individual notices | Pinned transparency post, updated as facts develop |

Practical rule: **work to the 72-hour GDPR clock.** Anything fast enough for GDPR satisfies the Texas pace comfortably.

**Phase 4 — Recover.** Eradicate the cause, restore from clean backups, rotate all secrets touched, re-enable services, monitor for recurrence.

**Phase 5 — Review (within 30 days of closure).** Post-incident review covering root cause, what worked, clock compliance, and corrective actions with owners and dates; report to the Board; update this plan; retain the incident record for 3 years minimum.

### B.4 Individual notice template (adapt per incident)

> Subject: Important security notice about your MonoGame community account
>
> What happened: On [date] we determined that [description] between [dates].
> What information was involved: [categories — e.g., email addresses, hashed passwords, IP addresses]. Payment card data was not involved; we never store it.
> What we have done: [containment, resets, patches, regulator notifications].
> What you can do: [reset password; beware of phishing emails referencing this incident; if reused elsewhere, change it there].
> What we will do next: [commitments]. Updates: [link to transparency post].
> Contact: [privacy@monogame.net](mailto:privacy@monogame.net). We are sorry this happened.

### B.5 Texas AG filing checklist (≥ 250 Texas residents)

1. Detailed description of the breach or its use of sensitive personal information.
2. Measures taken regarding the breach.
3. Measures intended after notification.
4. Whether law enforcement is investigating.
5. File via the AG's electronic submission form; keep the receipt in the incident record.

## Part C — Data Protection Operations

### C.1 Rights request (DSAR) procedure

1. **Intake:** all requests to `privacy@monogame.net`; log in the request register (date received, requester, type, deadline = one month).
2. **Verify identity:** confirmation email to the registered account address; for non-account requests, proportionate verification only — never ask for more data than needed.
3. **Execute:**
   - *Access/export:* point the user to Discourse self-service export, or run admin export.
   - *Correction:* user self-service, or admin edit.
   - *Deletion/anonymization:* Discourse admin → user → Anonymize (severs identity from posts, erases profile/email/IP links). Review the user's posts for embedded personal data on request and redact case by case. Note: public post text may lawfully remain (freedom-of-expression carve-out, content license), and this is stated in the Privacy Policy.
   - *Objection/restriction:* assess against legitimate interests; suspend the processing in question while assessing.
4. **Respond within one month**; one two-month extension is possible for complex requests — tell the requester within the first month with reasons. Refusals must cite the ground and mention complaint routes.
5. **Record** outcome and close the register entry. Target metric: 100% within deadline; review register quarterly.

### C.2 Privacy query handling (non-rights questions)

Community questions about data handling ("where is my data stored?", "do you sell data?", "what happens in a breach?") are answered from the Privacy Policy; if the policy doesn't answer it, that is a defect — answer the person, then file a policy amendment for the next review. Maintain a short public FAQ on the forum linking to the policy sections, including the breach commitments, so moderators can answer with a link.

### C.3 Retention and destruction schedule

Quarterly calendar task (Technical Lead):

- Verify Discourse settings enforce the published log retention (server logs ≤ 90 days; user/post IP history ≤ 5 years).
- Purge expired backups beyond the backup retention window.
- Destroy correspondence past its retention (privacy/DMCA: 3 years post-resolution) — destruction means erasure or rendering unreadable, per Tex. Bus. & Com. Code § 521.052(b).
- Confirm donation records held only as financial record-keeping requires.
- Record completion in the operations log.

### C.4 Vendor / processor management

- Maintain a register of every service holding personal data: name, role (processor vs independent controller), data categories, location, DPA status, breach-notification clause, sub-processor list link.
- Minimum contract terms for processors: process only on instruction; confidentiality; security measures; sub-processor flow-down; breach notice to us without undue delay; deletion/return at end of service; SCCs for EU/UK data.
- Review the register annually and when adding any new tool. Current entries to confirm at adoption: Discourse/CDCK (execute standard DPA if hosted), web/docs hosting, email delivery provider, storefront, payment processors.

### C.5 Security baseline (supports Tex. § 521.052(a) and GDPR Art. 32)

- MFA mandatory on: Discourse admin, DNS registrar, hosting consoles, GitHub organization owners, mail provider admin.
- Admin access list reviewed quarterly; offboard departing volunteers same week.
- TLS everywhere; HSTS on Foundation domains.
- Forum software updated within **`14`** days of security releases; critical patches ASAP.
- Backups encrypted at rest, access-restricted, restore-tested annually.
- Secrets (API keys, tokens) stored in a password manager/secret store, never in repositories.
- `privacy@monogame.net` monitored; good-faith vulnerability reports acknowledged within 5 business days (consider publishing a security.txt and simple safe-harbor disclosure policy).

### C.6 Annual review checklist

- [ ] Policies (ToS, Privacy) reviewed; version/changelog updated
- [ ] Legal scan: TDPSA scope changes, new state laws reaching nonprofits, COPPA/SCOPE developments
- [ ] Vendor register reviewed; DPAs current
- [ ] Retention settings audited against policy
- [ ] Admin access audit completed
- [ ] Tabletop exercise completed; corrective actions closed
- [ ] DMCA agent registration current, *if* the Board opted in to § 512 safe harbor (Copyright Office renewal every 3 years); otherwise confirm the published copyright-complaints contact still routes correctly
- [ ] Board briefed; minutes recorded

*This is an internal operating document. It is not legal advice; counsel should be engaged for any SEV-1 incident and for periodic review of this plan.*
