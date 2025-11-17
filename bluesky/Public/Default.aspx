<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="bluesky.Public.Default" %>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/css/home-default.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="bs-hero">
        <div class="container">
            <div class="row align-items-center">

                <div class="col-md-6 col-sm-12">
                    <span class="bs-hero-kicker">Portal de capacitación · BlueSky</span>
                    <h1 class="bs-hero-title">
                        Capacítate, rinde evaluaciones<br />
                        y obtén tus certificados en un solo lugar.
                    </h1>
                    <p class="bs-hero-subtitle">
                        Accede a cursos obligatorios y complementarios según tu rol,
                        con seguimiento de progreso y certificación automática.
                    </p>

                    <div class="bs-hero-actions">
                        <asp:Button ID="btnExplorarCursos"
                            runat="server"
                            Text="Ver cursos disponibles"
                            CssClass="btn btn-primary bs-btn-pill bs-hero-btn"
                            UseSubmitBehavior="false"
                            OnClick="btnExplorarCursos_Click" />

                        <a href="#como-funciona" class="btn btn-link bs-link-ghost">
                            ¿Cómo funciona?
                        </a>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="bs-hero-card">
                        <div class="bs-hero-tag">Panel administrador</div>
                        <h3>Vista rápida de cumplimiento</h3>
                        <ul class="bs-hero-list">
                            <li><i class="fa fa-check-circle"></i> Porcentaje de cursos completados</li>
                            <li><i class="fa fa-check-circle"></i> Evaluaciones aprobadas por área</li>
                            <li><i class="fa fa-check-circle"></i> Descarga de certificados en PDF</li>
                        </ul>
                        <div class="bs-hero-metrics">
                            <div>
                                <span class="bs-metric-label">Colaboradores activos</span>
                                <span class="bs-metric-value">124</span>
                            </div>
                            <div>
                                <span class="bs-metric-label">Tasa de aprobación</span>
                                <span class="bs-metric-value bs-metric-value--success">92%</span>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="bs-hero-bg-shape"></div>
    </section>

    <section id="como-funciona" class="bs-section bs-section-soft">
        <div class="container">
            <div class="bs-section-header text-center">
                <span class="bs-section-kicker">Flujo de aprendizaje</span>
                <h2 class="bs-section-title">Cómo funciona el portal</h2>
                <p class="bs-section-subtitle">
                    Sigue estos tres pasos para completar tus capacitaciones y mantenerte al día
                    con las políticas internas de BlueSky.
                </p>
            </div>

            <div class="row">

                <div class="col-md-4 col-sm-6">
                    <div class="bs-step-card">
                        <span class="bs-step-badge">01</span>
                        <h3>Revisa tus cursos asignados</h3>
                        <p>
                            Ingresa con tu cuenta corporativa y visualiza los cursos obligatorios
                            según tu cargo y área.
                        </p>
                        <span class="bs-chip">Onboarding</span>
                        <span class="bs-chip">Normativas internas</span>
                    </div>
                </div>

                <div class="col-md-4 col-sm-6">
                    <div class="bs-step-card">
                        <span class="bs-step-badge">02</span>
                        <h3>Realiza la capacitación y evaluación</h3>
                        <p>
                            Revisa el material del curso y luego rinde una evaluación en línea
                            para validar tus conocimientos.
                        </p>
                        <span class="bs-chip">Intentos controlados</span>
                        <span class="bs-chip">Feedback inmediato</span>
                    </div>
                </div>

                <div class="col-md-4 col-sm-6">
                    <div class="bs-step-card">
                        <span class="bs-step-badge">03</span>
                        <h3>Obtén tu certificado digital</h3>
                        <p>
                            Al aprobar, genera y descarga tu certificado en PDF,
                            disponible también para RRHH y jefaturas.
                        </p>
                        <span class="bs-chip">Certificado PDF</span>
                        <span class="bs-chip">Registro histórico</span>
                    </div>
                </div>

            </div>
        </div>
    </section>

    <section class="bs-section">
        <div class="container">
            <div class="bs-section-header">
                <span class="bs-section-kicker">Catálogo inicial</span>
                <h2 class="bs-section-title">Algunos cursos disponibles</h2>
            </div>

            <div class="row">

                <div class="col-md-4 col-sm-6">
                    <article class="bs-course-card">
                        <div class="bs-course-tag">Obligatorio</div>
                        <h3>Inducción Institucional</h3>
                        <p>
                            Conoce la historia, misión, visión y estructura de BlueSky para integrarte eficazmente.
                        </p>
                        <div class="bs-course-meta">
                            <span><i class="fa fa-clock-o"></i> 6 horas</span>
                            <span><i class="fa fa-calendar"></i> 2025</span>
                        </div>
                    </article>
                </div>

                <div class="col-md-4 col-sm-6">
                    <article class="bs-course-card">
                        <div class="bs-course-tag bs-course-tag--info">Normativas</div>
                        <h3>Políticas internas y cumplimiento</h3>
                        <p>
                            Revisa las políticas clave, responsabilidades y canales formales de comunicación.
                        </p>
                        <div class="bs-course-meta">
                            <span><i class="fa fa-clock-o"></i> 4 horas</span>
                            <span><i class="fa fa-calendar"></i> 2025</span>
                        </div>
                    </article>
                </div>

                <div class="col-md-4 col-sm-6">
                    <article class="bs-course-card">
                        <div class="bs-course-tag bs-course-tag--success">Recomendado</div>
                        <h3>Prevención de riesgos y seguridad</h3>
                        <p>
                            Aprende prácticas de seguridad, protocolos de emergencia
                            y cultura preventiva dentro de la organización.
                        </p>
                        <div class="bs-course-meta">
                            <span><i class="fa fa-clock-o"></i> 5 horas</span>
                            <span><i class="fa fa-calendar"></i> 2025</span>
                        </div>
                    </article>
                </div>

            </div>
        </div>
    </section>

    <section class="bs-section bs-section-dark">
        <div class="container">
            <div class="bs-section-header text-center">
                <span class="bs-section-kicker">Experiencia de los colaboradores</span>
                <h2 class="bs-section-title bs-section-title--light">
                    Lo que dicen sobre la plataforma
                </h2>
            </div>

            <div class="row">

                <div class="col-md-4 col-sm-6">
                    <article class="bs-testimonial-card">
                        <p class="bs-testimonial-text">
                            "En mi primer mes pude completar todos los cursos obligatorios
                            sin perderme entre correos ni archivos sueltos."
                        </p>
                        <div class="bs-testimonial-footer">
                            <span class="bs-testimonial-name">Camila · Analista de Riesgos</span>
                        </div>
                    </article>
                </div>

                <div class="col-md-4 col-sm-6">
                    <article class="bs-testimonial-card">
                        <p class="bs-testimonial-text">
                            "Como jefe de equipo, puedo revisar rápidamente quién está al día
                            con las capacitaciones y quién necesita apoyo."
                        </p>
                        <div class="bs-testimonial-footer">
                            <span class="bs-testimonial-name">Rodrigo · Jefe de Operaciones</span>
                        </div>
                    </article>
                </div>

                <div class="col-md-4 col-sm-6">
                    <article class="bs-testimonial-card">
                        <p class="bs-testimonial-text">
                            "Los certificados automáticos nos simplificaron mucho el trabajo
                            en RRHH al momento de auditorías internas."
                        </p>
                        <div class="bs-testimonial-footer">
                            <span class="bs-testimonial-name">Alejandra · RRHH</span>
                        </div>
                    </article>
                </div>

            </div>
        </div>
    </section>

    <section id="contacto" class="bs-section bs-section-soft">
        <div class="container">
            <div class="row">

                <div class="col-md-6 col-sm-12">
                    <div class="bs-contact-card">
                        <div class="bs-section-header">
                            <span class="bs-section-kicker">¿Necesitas ayuda?</span>
                            <h2 class="bs-section-title">Contáctanos</h2>
                            <p class="bs-section-subtitle">
                                Si tienes problemas con el acceso, cursos asignados
                                o evaluaciones, escríbenos y te apoyaremos.
                            </p>
                        </div>

                        <div class="bs-form-group">
                            <label for="contactName">Nombre completo</label>
                            <input type="text"
                                   class="form-control bs-input"
                                   id="contactName"
                                   placeholder="Ingresa tu nombre"
                                   name="contactName"
                                   required />
                        </div>

                        <div class="bs-form-group">
                            <label for="contactEmail">Correo electrónico</label>
                            <input type="email"
                                   class="form-control bs-input"
                                   id="contactEmail"
                                   placeholder="tu.correo@bluesky.cl"
                                   name="contactEmail"
                                   required />
                        </div>

                        <div class="bs-form-group">
                            <label for="txtMotivo">Mensaje</label>
                            <asp:TextBox ID="txtMotivo"
                                runat="server"
                                ClientIDMode="Static"
                                CssClass="form-control bs-input bs-input-textarea"
                                TextMode="MultiLine"
                                placeholder="Describe brevemente tu consulta o problema"
                                Rows="4"></asp:TextBox>
                        </div>

                        <div class="bs-form-actions">
                            <asp:Button ID="btnContact"
                                runat="server"
                                ClientIDMode="Static"
                                Text="Enviar mensaje"
                                CssClass="btn btn-primary bs-btn-pill"
                                UseSubmitBehavior="false" />
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="bs-contact-side">
                        <h3>Información útil</h3>
                        <ul class="bs-contact-list">
                            <li><i class="fa fa-clock-o"></i> Horario de soporte: Lunes a Viernes, 09:00 a 18:00 hrs.</li>
                            <li><i class="fa fa-envelope-o"></i> Soporte interno: soporte.capacitacion@bluesky.cl</li>
                            <li><i class="fa fa-info-circle"></i> Usa tu correo corporativo para iniciar sesión.</li>
                        </ul>

                        <div class="bs-contact-highlight">
                            <p>
                                Recuerda completar tus cursos obligatorios dentro de los plazos definidos por RRHH
                                para mantener tu perfil al día.
                            </p>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

</asp:Content>
